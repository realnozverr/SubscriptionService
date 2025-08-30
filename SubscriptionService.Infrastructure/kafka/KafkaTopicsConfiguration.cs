namespace SubscriptionService.Infrastructure.kafka;

public class KafkaTopicsConfiguration
{
    public required string SubscriptionCreatedTopic { get; init; }
    public required string SubscriptionCanceledTopic { get; init; }
    public required string SubscriptionExpiredTopic { get; init; }
}