#nullable enable

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace VisualAcademy.Infrastructures;

/// <summary>
/// 모든 Tenant 데이터베이스에 HR Employment Information 전용
/// Audit Header/Detail 테이블과 관련 인덱스 및 FK를 생성합니다.
/// </summary>
public sealed class TenantSchemaEnhancerCreateEmployeeEmploymentAuditTables
{
    private const int CommandTimeoutSeconds = 60;

    private readonly string _masterConnectionString;
    private readonly ILogger<TenantSchemaEnhancerCreateEmployeeEmploymentAuditTables> _logger;

    public TenantSchemaEnhancerCreateEmployeeEmploymentAuditTables(
        string masterConnectionString,
        ILogger<TenantSchemaEnhancerCreateEmployeeEmploymentAuditTables> logger)
    {
        if (string.IsNullOrWhiteSpace(masterConnectionString))
        {
            throw new ArgumentException(
                "The master connection string is required.",
                nameof(masterConnectionString));
        }

        _masterConnectionString = masterConnectionString;
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Master DB의 Tenants 테이블에서 Tenant 연결 문자열을 읽고,
    /// 모든 Tenant DB의 Audit 테이블 스키마를 보정합니다.
    /// </summary>
    public void EnhanceAllTenantDatabases()
    {
        List<(string TenantName, string ConnectionString)> tenants;

        try
        {
            tenants = GetTenantConnectionStrings();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to retrieve tenant connection strings while creating Employee Employment Audit tables.");

            return;
        }

        foreach (var tenant in tenants)
        {
            if (ShouldSkipTenant(
                tenant.TenantName,
                tenant.ConnectionString))
            {
                continue;
            }

            try
            {
                EnsureEmployeeEmploymentAuditTables(
                    tenant.ConnectionString);
            }
            catch (Exception ex)
            {
                /*
                 * ConnectionString에는 계정이나 비밀번호가 포함될 수 있으므로
                 * 로그에는 TenantName만 남깁니다.
                 */
                _logger.LogError(
                    ex,
                    "Failed to create Employee Employment Audit tables. TenantName={TenantName}",
                    tenant.TenantName);
            }
        }
    }

    /// <summary>
    /// Master DB의 dbo.Tenants에서 Tenant 이름과 연결 문자열을 조회합니다.
    /// </summary>
    private List<(string TenantName, string ConnectionString)>
        GetTenantConnectionStrings()
    {
        var result =
            new List<(string TenantName, string ConnectionString)>();

        using var connection =
            new SqlConnection(_masterConnectionString);

        connection.Open();

        const string sql = @"
SELECT
    [Name],
    [ConnectionString]
FROM [dbo].[Tenants]
WHERE [ConnectionString] IS NOT NULL
  AND LTRIM(RTRIM([ConnectionString])) <> '';";

        using var command =
            new SqlCommand(sql, connection)
            {
                CommandTimeout = CommandTimeoutSeconds
            };

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var tenantName =
                reader["Name"]?.ToString()?.Trim()
                ?? string.Empty;

            var connectionString =
                reader["ConnectionString"]?.ToString()?.Trim();

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                result.Add((
                    tenantName,
                    connectionString));
            }
        }

        return result;
    }

    /// <summary>
    /// 삭제되었거나 접속해서는 안 되는 Tenant DB를 제외합니다.
    /// </summary>
    private static bool ShouldSkipTenant(
        string? tenantName,
        string? connectionString)
    {
        if (string.Equals(
            tenantName,
            "ShakopeeTenant",
            StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        if (!string.IsNullOrWhiteSpace(connectionString)
            && connectionString.IndexOf(
                "ShakopeeTenantUser",
                StringComparison.OrdinalIgnoreCase) >= 0)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 한 Tenant DB 안에서 Header, Detail, FK, Index를
    /// 하나의 트랜잭션으로 생성합니다.
    /// </summary>
    private static void EnsureEmployeeEmploymentAuditTables(
        string connectionString)
    {
        using var connection =
            new SqlConnection(connectionString);

        connection.Open();

        using var transaction =
            connection.BeginTransaction();

        try
        {
            /*
             * FK 대상인 Records 테이블을 먼저 생성해야 합니다.
             */
            EnsureAuditRecordsTable(
                connection,
                transaction);

            EnsureAuditChangesTable(
                connection,
                transaction);

            EnsureAuditChangesForeignKey(
                connection,
                transaction);

            EnsureAuditIndexes(
                connection,
                transaction);

            transaction.Commit();
        }
        catch
        {
            try
            {
                transaction.Rollback();
            }
            catch
            {
                // 원래 발생한 스키마 생성 예외를 유지합니다.
            }

            throw;
        }
    }

    /// <summary>
    /// 한 번의 Employment Information 저장 작업을 나타내는
    /// Audit Header 테이블을 생성합니다.
    /// </summary>
    private static void EnsureAuditRecordsTable(
        SqlConnection connection,
        SqlTransaction transaction)
    {
        const string sql = @"
IF OBJECT_ID(
    N'[dbo].[EmployeeEmploymentAuditRecords]',
    N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[EmployeeEmploymentAuditRecords]
    (
        [ID] BIGINT IDENTITY(1, 1) NOT NULL
            CONSTRAINT [PK_EmployeeEmploymentAuditRecords]
            PRIMARY KEY,

        [AuditTrailRecordID] BIGINT NOT NULL,
        [EmployeeID] BIGINT NOT NULL,
        [EmployeeName] NVARCHAR(300) NOT NULL,

        [ChangedByUserID] NVARCHAR(450) NULL,
        [ChangedByUserName] NVARCHAR(256) NOT NULL,
        [ChangedByEmail] NVARCHAR(256) NULL,
        [ChangedByRole] NVARCHAR(100) NULL,

        [ChangedAt] DATETIMEOFFSET NOT NULL,
        [ChangeCount] INT NOT NULL,

        [TraceIdentifier] NVARCHAR(100) NULL,
        [RequestPath] NVARCHAR(500) NULL,
        [RemoteIpAddress] NVARCHAR(64) NULL,

        [Active] BIT NOT NULL
            CONSTRAINT [DF_EmployeeEmploymentAuditRecords_Active]
            DEFAULT (1)
    );
END;";

        ExecuteNonQuery(
            connection,
            transaction,
            sql);
    }

    /// <summary>
    /// Audit Header 한 건에 포함된 실제 필드 변경 내역을 저장하는
    /// Audit Detail 테이블을 생성합니다.
    /// </summary>
    private static void EnsureAuditChangesTable(
        SqlConnection connection,
        SqlTransaction transaction)
    {
        const string sql = @"
IF OBJECT_ID(
    N'[dbo].[EmployeeEmploymentAuditChanges]',
    N'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[EmployeeEmploymentAuditChanges]
    (
        [ID] BIGINT IDENTITY(1, 1) NOT NULL
            CONSTRAINT [PK_EmployeeEmploymentAuditChanges]
            PRIMARY KEY,

        [AuditRecordID] BIGINT NOT NULL,

        [PropertyName] NVARCHAR(100) NOT NULL,
        [DisplayName] NVARCHAR(150) NOT NULL,

        [OldValue] NVARCHAR(MAX) NULL,
        [NewValue] NVARCHAR(MAX) NULL,

        [OldDisplayValue] NVARCHAR(500) NULL,
        [NewDisplayValue] NVARCHAR(500) NULL,

        [DataType] NVARCHAR(50) NOT NULL,
        [SortOrder] INT NOT NULL,
        [ChangedAt] DATETIMEOFFSET NOT NULL,

        [Active] BIT NOT NULL
            CONSTRAINT [DF_EmployeeEmploymentAuditChanges_Active]
            DEFAULT (1)
    );
END;";

        ExecuteNonQuery(
            connection,
            transaction,
            sql);
    }

    /// <summary>
    /// 기존에 Detail 테이블만 생성되어 있거나 FK가 누락된 환경도
    /// 보정할 수 있도록 FK 존재 여부를 별도로 검사합니다.
    /// </summary>
    private static void EnsureAuditChangesForeignKey(
        SqlConnection connection,
        SqlTransaction transaction)
    {
        const string sql = @"
IF OBJECT_ID(
       N'[dbo].[EmployeeEmploymentAuditRecords]',
       N'U') IS NOT NULL
   AND OBJECT_ID(
       N'[dbo].[EmployeeEmploymentAuditChanges]',
       N'U') IS NOT NULL
   AND NOT EXISTS
   (
       SELECT 1
       FROM [sys].[foreign_keys]
       WHERE [name] =
           N'FK_EmployeeEmploymentAuditChanges_Record'
         AND [parent_object_id] =
           OBJECT_ID(
               N'[dbo].[EmployeeEmploymentAuditChanges]')
   )
BEGIN
    ALTER TABLE [dbo].[EmployeeEmploymentAuditChanges]
        WITH CHECK
        ADD CONSTRAINT
            [FK_EmployeeEmploymentAuditChanges_Record]
        FOREIGN KEY ([AuditRecordID])
        REFERENCES
            [dbo].[EmployeeEmploymentAuditRecords] ([ID])
        ON DELETE CASCADE;

    ALTER TABLE [dbo].[EmployeeEmploymentAuditChanges]
        CHECK CONSTRAINT
            [FK_EmployeeEmploymentAuditChanges_Record];
END;";

        ExecuteNonQuery(
            connection,
            transaction,
            sql);
    }

    /// <summary>
    /// 목록, 상세 조회 및 기존 AuditTrail 연결에 필요한
    /// 인덱스를 생성합니다.
    /// </summary>
    private static void EnsureAuditIndexes(
        SqlConnection connection,
        SqlTransaction transaction)
    {
        const string sql = @"
IF NOT EXISTS
(
    SELECT 1
    FROM [sys].[indexes]
    WHERE [name] =
        N'UX_EmployeeEmploymentAuditRecords_AuditTrailRecordID'
      AND [object_id] =
        OBJECT_ID(
            N'[dbo].[EmployeeEmploymentAuditRecords]')
)
BEGIN
    CREATE UNIQUE INDEX
        [UX_EmployeeEmploymentAuditRecords_AuditTrailRecordID]
    ON [dbo].[EmployeeEmploymentAuditRecords]
        ([AuditTrailRecordID]);
END;

IF NOT EXISTS
(
    SELECT 1
    FROM [sys].[indexes]
    WHERE [name] =
        N'IX_EmployeeEmploymentAuditRecords_EmployeeID_ChangedAt'
      AND [object_id] =
        OBJECT_ID(
            N'[dbo].[EmployeeEmploymentAuditRecords]')
)
BEGIN
    CREATE INDEX
        [IX_EmployeeEmploymentAuditRecords_EmployeeID_ChangedAt]
    ON [dbo].[EmployeeEmploymentAuditRecords]
        ([EmployeeID], [ChangedAt] DESC);
END;

IF NOT EXISTS
(
    SELECT 1
    FROM [sys].[indexes]
    WHERE [name] =
        N'IX_EmployeeEmploymentAuditRecords_ChangedByUserName_ChangedAt'
      AND [object_id] =
        OBJECT_ID(
            N'[dbo].[EmployeeEmploymentAuditRecords]')
)
BEGIN
    CREATE INDEX
        [IX_EmployeeEmploymentAuditRecords_ChangedByUserName_ChangedAt]
    ON [dbo].[EmployeeEmploymentAuditRecords]
        ([ChangedByUserName], [ChangedAt] DESC);
END;

IF NOT EXISTS
(
    SELECT 1
    FROM [sys].[indexes]
    WHERE [name] =
        N'IX_EmployeeEmploymentAuditChanges_AuditRecordID'
      AND [object_id] =
        OBJECT_ID(
            N'[dbo].[EmployeeEmploymentAuditChanges]')
)
BEGIN
    CREATE INDEX
        [IX_EmployeeEmploymentAuditChanges_AuditRecordID]
    ON [dbo].[EmployeeEmploymentAuditChanges]
        ([AuditRecordID], [SortOrder]);
END;";

        ExecuteNonQuery(
            connection,
            transaction,
            sql);
    }

    private static void ExecuteNonQuery(
        SqlConnection connection,
        SqlTransaction transaction,
        string sql)
    {
        using var command =
            new SqlCommand(
                sql,
                connection,
                transaction)
            {
                CommandTimeout = CommandTimeoutSeconds
            };

        command.ExecuteNonQuery();
    }

    /// <summary>
    /// Startup.cs에서 간단히 호출하기 위한 진입점입니다.
    /// </summary>
    public static void Run(IServiceProvider services)
    {
        ArgumentNullException.ThrowIfNull(services);

        try
        {
            var configuration =
                services.GetRequiredService<IConfiguration>();

            var logger =
                services.GetRequiredService<
                    ILogger<
                        TenantSchemaEnhancerCreateEmployeeEmploymentAuditTables>>();

            var masterConnectionString =
                configuration.GetConnectionString(
                    "DefaultConnection");

            if (string.IsNullOrWhiteSpace(
                masterConnectionString))
            {
                logger.LogError(
                    "Employee Employment Audit table creation was skipped because DefaultConnection is not configured.");

                return;
            }

            var enhancer =
                new TenantSchemaEnhancerCreateEmployeeEmploymentAuditTables(
                    masterConnectionString,
                    logger);

            enhancer.EnhanceAllTenantDatabases();
        }
        catch (Exception ex)
        {
            var fallbackLogger =
                services.GetService<
                    ILogger<
                        TenantSchemaEnhancerCreateEmployeeEmploymentAuditTables>>();

            fallbackLogger?.LogError(
                ex,
                "An error occurred while creating Employee Employment Audit tables.");
        }
    }
}