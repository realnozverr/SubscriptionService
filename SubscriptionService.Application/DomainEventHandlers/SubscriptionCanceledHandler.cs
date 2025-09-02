using MediatR;
using SubscriptionService.Application.Abstractions.Kafka;
using SubscriptionService.Domain.Entities.SubscriptionAggregate.DomainEvents;

namespace SubscriptionService.Application.DomainEventHandlers;

public class SubscriptionCanceledHandler(IMessageBus messageBus)
    : INotificationHandler<SubscriptionCanceledEvent>
{
    public async Task Handle(SubscriptionCanceledEvent notification, CancellationToken cancellationToken)
    {
        await messageBus.Publish(notification, cancellationToken);
    }
}