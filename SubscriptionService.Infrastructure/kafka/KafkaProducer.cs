using MassTransit;
using Microsoft.Extensions.Options;
using SubscriptionKafkaContracts.From.SubscriptionKafkaEvents;
using SubscriptionService.Application.Abstractions.Kafka;
using SubscriptionService.Domain.Entities.SubscriptionAggregate.DomainEvents;

namespace SubscriptionService.Infrastructure.kafka;

public class KafkaProducer(
    ITopicProducerProvider topicProducerProvider,
    IOptions<KafkaTopicsConfiguration> topicsConfiguration) : IMessageBus
{
    private readonly KafkaTopicsConfiguration _topicsConfiguration = topicsConfiguration.Value;

    public async Task Publish(SubscriptionCreatedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var producer = topicProducerProvider.GetProducer<string, SubscriptionCreated>(
            new Uri($"topic:{_topicsConfiguration.SubscriptionCreatedTopic}"));

        await producer.Produce(domainEvent.EventId.ToString(), new SubscriptionCreated(
                eventId: domainEvent.EventId,
                userId: domainEvent.UserId,
                planId: domainEvent.PlanId,
                endDate: domainEvent.EndDate),
            cancellationToken);
    }

    public async Task Publish(SubscriptionCanceledEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var producer = topicProducerProvider.GetProducer<string, SubscriptionCanceled>(
            new Uri($"topic:{_topicsConfiguration.SubscriptionCanceledTopic}"));

        await producer.Produce(domainEvent.EventId.ToString(), new SubscriptionCanceled(
                eventId: domainEvent.EventId,
                subscriptionId: domainEvent.SubscriptionId,
                userId: domainEvent.UserId),
            cancellationToken);
    }

    public async Task Publish(SubscriptionExpiredEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var producer = topicProducerProvider.GetProducer<string, SubscriptionExpired>(
            new Uri($"topic:{_topicsConfiguration.SubscriptionExpiredTopic}"));

        await producer.Produce(domainEvent.EventId.ToString(), new SubscriptionExpired(
                eventId: domainEvent.EventId,
                subscriptionId: domainEvent.SubscriptionId,
                userId: domainEvent.UserId),
            cancellationToken);
    }
}