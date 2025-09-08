using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Time.Testing;
using Moq;
using SubscriptionService.Application.UseCases.Commands.CreateSubscriptionCommand;
using SubscriptionService.Domain.Abstractions;
using SubscriptionService.Domain.Abstractions.Repositories;
using SubscriptionService.Domain.Entities.PlanAggregate;
using SubscriptionService.Domain.Entities.SubscriptionAggregate;
using SubscriptionService.Domain.Entities.UserAggregate;

namespace UnitTests.Application.UseCases.CreateSubscription;

[TestSubject(typeof(CreateSubscriptionHandler))]
public class CreateSubscriptionHandlerShould
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IPlanRepository> _planRepositoryMock;
    private readonly Mock<ISubscriptionRepository> _subscriptionRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly FakeTimeProvider _timeProvider;
    private readonly CreateSubscriptionHandler _handler;

    public CreateSubscriptionHandlerShould()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _planRepositoryMock = new Mock<IPlanRepository>();
        _subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _timeProvider = new FakeTimeProvider(new DateTimeOffset(2025, 1, 1, 0, 0, 0, 0, TimeSpan.Zero));

        _handler = new CreateSubscriptionHandler(
            _userRepositoryMock.Object,
            _planRepositoryMock.Object,
            _subscriptionRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _timeProvider
        );
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_WhenUserAndPlanExistAndNoActiveSubscription()
    {
        //Arrange
        var user = User.Create(TelegramId.Create(123), "vpnId","test_name", UserStatus.Active);
        var plan = Plan.Monthly;
        var command = new CreateSubscriptionCommand(user.Id, plan.Id);

        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _planRepositoryMock
            .Setup(r => r.GetByIdAsync(command.PlanId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(plan);

        _subscriptionRepositoryMock
            .Setup(r => r.GetActiveSubscriptionByUserIdAsync(command.UserId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Subscription?)null);

        //Act
        var result = await _handler.Handle(command, CancellationToken.None);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.SubscriptionId.Should().NotBeEmpty();

        _subscriptionRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Subscription>(), It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWorkMock.Verify(uow => uow.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}