using SubscriptionService.Domain.SeedWork;

namespace SubscriptionService.Domain.Entities.SubscriptionAggregate.DomainEvents;

public record SubscriptionExpiredEvent(
    Guid SubscriptionId,
    Guid UserId
) : DomainEvent;