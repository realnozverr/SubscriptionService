using MediatR;

namespace SubscriptionService.Domain.SeedWork;

public abstract record DomainEvent : INotification
{
    public Guid EventId { get; protected set; } = Guid.NewGuid();
}