namespace SubscriptionService.Domain.SeedWork;

public abstract class Aggregate<Tid> : Entity<Tid>, IAggregate
    where Tid : IComparable<Tid>
{
    private readonly List<DomainEvent> _domainEvents = [];
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}