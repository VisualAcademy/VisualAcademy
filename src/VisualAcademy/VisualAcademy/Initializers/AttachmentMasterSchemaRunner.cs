using Azunt.AttachmentManagement;

namespace Azunt.Initializers;

/// <summary>
/// 마스터 데이터베이스의 dbo.Attachments 테이블을 생성하거나
/// 기존 테이블에 누락된 컬럼을 추가합니다.
/// </summary>
public static class AttachmentMasterSchemaRunner
{
    public static async Task RunAsync(
        IServiceProvider services,
        string masterConnectionString,
        bool ensureIndexes = false,
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
            nameof(AttachmentMasterSchemaRunner));

        var tableBuilder = scope.ServiceProvider
            .GetRequiredService<AttachmentsTableBuilder>();

        try
        {
            logger.LogInformation(
                "Starting Attachments schema initialization for the master database.");

            await tableBuilder.EnsureAsync(
                connectionString: masterConnectionString,
                ensureIndexes: ensureIndexes,
                cancellationToken: cancellationToken);

            logger.LogInformation(
                "Attachments schema initialization completed for the master database.");
        }
        catch (OperationCanceledException)
            when (cancellationToken.IsCancellationRequested)
        {
            logger.LogWarning(
                "Attachments schema initialization was canceled for the master database.");

            throw;
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Attachments schema initialization failed for the master database.");

            throw;
        }
    }
}