namespace SubscriptionService.Application.Abstractions.Kafka;

public interface IMessageBus
{
    Task Publish<T>(T message, CancellationToken cancellationToken = default);
}