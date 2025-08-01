using SubscriptionService.Domain.SeedWork;

namespace SubscriptionService.Domain.Entities.SubscriptionAggregate.DomainEvents;

public record SubscriptionCanceledEvent(
    Guid SubscriptionId,
    Guid UserId
) : DomainEvent;