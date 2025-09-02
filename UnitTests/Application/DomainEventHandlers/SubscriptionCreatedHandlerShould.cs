using JetBrains.Annotations;
using SubscriptionService.Application.DomainEventHandlers;
using SubscriptionService.Domain.Entities.SubscriptionAggregate.DomainEvents;

namespace UnitTests.Application.DomainEventHandlers;

[TestSubject(typeof(SubscriptionCreatedHandler))]
public class SubscriptionCreatedHandlerShould
{
    private readonly SubscriptionCreatedEvent _domainEvent = new(
        Guid.NewGuid(),
        Guid.NewGuid(), 
        1,
        DateTime.UtcNow);

    [Fact]
    public async Task PublishCallMessageBus()
    {
        //Arrange
    }
}