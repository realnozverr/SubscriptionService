using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quartz;
using SubscriptionService.Domain.Entities.SubscriptionAggregate;

namespace SubscriptionService.Infrastructure.Postgres.BackgroundJobs;

[DisallowConcurrentExecution]
public sealed class CheckExpiredSubscriptionsBackgroundJob : IJob
{
    private readonly IDbContextFactory<DataContext> _contextFactory;
    private readonly ILogger<CheckExpiredSubscriptionsBackgroundJob> _logger;

    public CheckExpiredSubscriptionsBackgroundJob(
        IDbContextFactory<DataContext> contextFactory,
        ILogger<CheckExpiredSubscriptionsBackgroundJob> logger)
    {
        _contextFactory = contextFactory;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Starting to check for expired subscriptions...");
        await using var dbContext = await _contextFactory.CreateDbContextAsync(context.CancellationToken);

        var expiresSubscriptions = await GetSubscriptionsToExpireAsync(dbContext, context.CancellationToken);

        if (expiresSubscriptions.Count == 0) return;

        _logger.LogInformation("Processing a batch of {Count} subscriptions...", expiresSubscriptions.Count);

        foreach (var expiredSubscription in expiresSubscriptions)
        {
            expiredSubscription.Expire();
        }

        dbContext.UpdateRange(expiresSubscriptions);

        await dbContext.SaveChangesAsync(context.CancellationToken);
        _logger.LogInformation("Successfully processed a batch of {Count} subscriptions.", expiresSubscriptions.Count);
    }

    private static async Task<List<Subscription>> GetSubscriptionsToExpireAsync(
        DataContext dbContext,
        CancellationToken ct)
    {
        const int batchSize = 3000;
        var currentTime = DateTime.UtcNow;

        return await dbContext.Subscriptions
            .FromSqlInterpolated($@"
                SELECT * FROM ""subscriptions""
                WHERE ""status"" = 1 AND ""end_date"" <= {currentTime}
                ORDER BY ""end_date""
                LIMIT {batchSize}
                FOR UPDATE SKIP LOCKED")
            .AsNoTracking()
            .ToListAsync(ct);
    }
}