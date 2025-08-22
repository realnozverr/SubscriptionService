using MediatR;
using SubscriptionService.Application.Abstractions.Kafka;
using SubscriptionService.Domain.Entities.SubscriptionAggregate.DomainEvents;

namespace SubscriptionService.Application.DomainEventHandlers;

public class SubscriptionExpiredHandler(IMessageBus messageBus)
    : INotificationHandler<SubscriptionExpiredEvent>
{
    public async Task Handle(SubscriptionExpiredEvent notification, CancellationToken cancellationToken)
    {
        await messageBus.Publish(notification, cancellationToken);
    }
}