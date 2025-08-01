using SubscriptionService.Domain.SeedWork;

namespace SubscriptionService.Domain.Entities.SubscriptionAggregate.DomainEvents;

public record SubscriptionCreatedEvent(
    Guid SubscriptionId,
    Guid UserId,
    Guid PlanId,
    DateTime EndDate
) : DomainEvent;