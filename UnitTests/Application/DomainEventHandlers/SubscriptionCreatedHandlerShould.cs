using JetBrains.Annotations;
using Moq;
using SubscriptionService.Application.Abstractions.Kafka;
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
        "telegram_name",
        DateTime.UtcNow);

    [Fact]
    public async Task PublishCallMessageBus()
    {
        //Arrange
        var messageBusMock = new Mock<IMessageBus>();
        var handler = new SubscriptionCreatedHandler(messageBusMock.Object);
        
        //Act
        await handler.Handle(_domainEvent, CancellationToken.None);
        
        //Assert
        messageBusMock.Verify(x => x.Publish(_domainEvent, It.IsAny<CancellationToken>()), Times.Once);
    }
}