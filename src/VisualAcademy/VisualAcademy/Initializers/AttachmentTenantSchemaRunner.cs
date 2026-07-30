using Azunt.AttachmentManagement;

namespace Azunt.Initializers;

/// <summary>
/// 마스터 데이터베이스의 dbo.Tenants 테이블에서 연결 문자열을 조회한 후
/// 각 테넌트 데이터베이스의 dbo.Attachments 테이블을 생성하거나 확장합니다.
/// </summary>
public static class AttachmentTenantSchemaRunner
{
    public static async Task RunAsync(
        IServiceProvider services,
        string masterConnectionString,
        bool ensureIndexes = false,
        bool failFast = true,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(services);

        if (string.IsNullOrWhiteSpace(masterConnectionString))
        {
            throw new ArgumentException(
                "Master connection string is required.",
                nameof(masterConnectionString));
        }

        await using var scope = services.CreateAsyncScope();

        var loggerFactory = scope.ServiceProvider
            .GetRequiredService<ILoggerFactory>();

        var logger = loggerFactory.CreateLogger(
            nameof(AttachmentTenantSchemaRunner));

        var tableBuilder = scope.ServiceProvider
            .GetRequiredService<AttachmentsTableBuilder>();

        IReadOnlyList<string> tenantConnectionStrings;

        try
        {
            tenantConnectionStrings =
                await GetTenantConnectionStringsAsync(
                    masterConnectionString,
                    cancellationToken);
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            logger.LogWarning(
                "Loading tenant database connection strings was canceled.");

            throw;
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Failed to load tenant database connection strings.");

            throw;
        }

        logger.LogInformation(
            "Starting Attachments schema initialization for {TenantCount} tenant databases.",
            tenantConnectionStrings.Count);

        var succeededCount = 0;
        var failedCount = 0;

        for (var index = 0;
             index < tenantConnectionStrings.Count;
             index++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var tenantNumber = index + 1;
            var tenantConnectionString =
                tenantConnectionStrings[index];

            try
            {
                await tableBuilder.EnsureAsync(
                    connectionString: tenantConnectionString,
                    ensureIndexes: ensureIndexes,
                    cancellationToken: cancellationToken);

                succeededCount++;

                logger.LogInformation(
                    "Attachments schema initialization completed for tenant database #{TenantNumber}.",
                    tenantNumber);
            }
            catch (OperationCanceledException)
                when (cancellationToken.IsCancellationRequested)
            {
                logger.LogWarning(
                    "Attachments schema initialization was canceled while processing tenant database #{TenantNumber}.",
                    tenantNumber);

                throw;
            }
            catch (Exception exception)
            {
                failedCount++;

                // 연결 문자열과 실제 DB 이름은 로그에 남기지 않습니다.
                logger.LogError(
                    exception,
                    "Attachments schema initialization failed for tenant database #{TenantNumber}.",
                    tenantNumber);

                if (failFast)
                {
                    throw;
                }
            }
        }

        logger.LogInformation(
            "Tenant Attachments schema initialization finished. Succeeded: {SucceededCount}, Failed: {FailedCount}.",
            succeededCount,
            failedCount);
    }

    /// <summary>
    /// 마스터 DB의 dbo.Tenants 테이블에서
    /// 유효한 테넌트 연결 문자열을 가져옵니다.
    /// </summary>
    private static async Task<IReadOnlyList<string>>
        GetTenantConnectionStringsAsync(
            string masterConnectionString,
            CancellationToken cancellationToken)
    {
        var connectionStrings =
            new HashSet<string>(
                StringComparer.OrdinalIgnoreCase);

        await using var connection =
            new SqlConnection(masterConnectionString);

        await connection.OpenAsync(cancellationToken);

        const string sql = """
            SELECT DISTINCT [ConnectionString]
            FROM [dbo].[Tenants]
            WHERE [ConnectionString] IS NOT NULL
              AND LTRIM(RTRIM([ConnectionString])) <> '';
            """;

        await using var command =
            new SqlCommand(sql, connection)
            {
                CommandTimeout = 60,
            };

        await using var reader =
            await command.ExecuteReaderAsync(
                cancellationToken);

        var connectionStringOrdinal =
            reader.GetOrdinal("ConnectionString");

        while (await reader.ReadAsync(cancellationToken))
        {
            if (reader.IsDBNull(connectionStringOrdinal))
            {
                continue;
            }

            var connectionString =
                reader.GetString(connectionStringOrdinal)
                    .Trim();

            if (!string.IsNullOrWhiteSpace(connectionString))
            {
                connectionStrings.Add(connectionString);
            }
        }

        return connectionStrings.ToList();
    }
}