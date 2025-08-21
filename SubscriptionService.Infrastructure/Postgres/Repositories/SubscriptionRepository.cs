using Microsoft.EntityFrameworkCore;
using SubscriptionService.Domain.Abstractions.Repositories;
using SubscriptionService.Domain.Entities.SubscriptionAggregate;

namespace SubscriptionService.Infrastructure.Postgres.Repositories;

public class SubscriptionRepository(DataContext context) : ISubscriptionRepository
{
    public async Task<Subscription?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Subscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Subscription>> GetByUserIdAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await context.Subscriptions
            .AsNoTracking()
            .Where(s => s.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Subscription?> GetActiveSubscriptionByUserIdAsync(Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await context.Subscriptions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.UserId == userId && s.Status == SubscriptionStatus.Active, cancellationToken);
    }

    public async Task<IEnumerable<Subscription>> GetSubscriptionsToExpireAsync(DateTime date,
        CancellationToken cancellationToken = default)
    {
        return await context.Subscriptions
            .Where(s => s.Status == SubscriptionStatus.Active && s.EndDate <= date)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default)
    {
        await context.Subscriptions.AddAsync(subscription, cancellationToken);
    }

    public void Update(Subscription subscription)
    {
        context.Subscriptions.Update(subscription);
    }
}