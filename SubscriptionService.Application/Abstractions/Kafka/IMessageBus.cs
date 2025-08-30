using SubscriptionService.Domain.Entities.SubscriptionAggregate.DomainEvents;

namespace SubscriptionService.Application.Abstractions.Kafka;

public interface IMessageBus
{
    Task Publish(SubscriptionCreatedEvent message, CancellationToken cancellationToken = default);
    Task Publish(SubscriptionCanceledEvent message, CancellationToken cancellationToken = default);

    Task Publish(SubscriptionExpiredEvent message, CancellationToken cancellationToken = default);

}