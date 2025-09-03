using JetBrains.Annotations;
using Moq;
using SubscriptionService.Application.Abstractions.Kafka;
using SubscriptionService.Application.DomainEventHandlers;
using SubscriptionService.Domain.Entities.SubscriptionAggregate.DomainEvents;

namespace UnitTests.Application.DomainEventHandlers;

[TestSubject(typeof(SubscriptionCanceledHandler))]
public class SubscriptionCanceledHandlerShould
{
    private readonly SubscriptionCanceledEvent _domainEvent = new(Guid.NewGuid(), Guid.NewGuid());
    [Fact]
    public async Task PublishCallMessageBus()
    {
        //Arrange
        var messageBusMock = new Mock<IMessageBus>();
        var handler = new SubscriptionCanceledHandler(messageBusMock.Object);
        
        //Act
        await handler.Handle(_domainEvent, CancellationToken.None);
        
        //Assert
        messageBusMock.Verify(x => x.Publish(_domainEvent, It.IsAny<CancellationToken>()), Times.Once);
    }
}