using MediatR;
using SubscriptionService.Application.Abstractions.Kafka;
using SubscriptionService.Domain.Entities.SubscriptionAggregate.DomainEvents;

namespace SubscriptionService.Application.DomainEventHandlers;

public class SubscriptionCreatedHandler(IMessageBus messageBus)
: INotificationHandler<SubscriptionCreatedEvent>
{
    public async Task Handle(SubscriptionCreatedEvent @event, CancellationToken cancellationToken)
    {
        await messageBus.Publish(@event, cancellationToken);
    }
}