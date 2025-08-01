namespace SubscriptionService.Domain.SeedWork;

public interface IAggregate
{
    IReadOnlyList<DomainEvent> DomainEvents { get; }

    public void ClearDomainEvents();
}