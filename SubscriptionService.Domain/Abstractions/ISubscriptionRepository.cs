using SubscriptionService.Domain.Entities.SubscriptionAggregate;

namespace SubscriptionService.Domain.Abstractions;

public interface ISubscriptionRepository : IRepository<Subscription>
{
}