using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Time.Testing;
using Moq;
using SubscriptionService.Application.UseCases.Queries.GetSubscriptionStatusQuery;
using SubscriptionService.Domain.Abstractions.Repositories;
using SubscriptionService.Domain.Entities.PlanAggregate;
using SubscriptionService.Domain.Entities.SubscriptionAggregate;
using SubscriptionService.Domain.Entities.UserAggregate;

namespace UnitTests.Application.UseCases.SubscriptionStatus;

[TestSubject(typeof(GetSubscriptionStatusHandler))]
public class GetSubscriptionStatusHandlerShould
{
    private readonly Mock<ISubscriptionRepository> _subscriptionRepositoryMock;
    private readonly Mock<IPlanRepository> _planRepositoryMock;
    private readonly FakeTimeProvider _timeProvider;
    private readonly GetSubscriptionStatusHandler _handler;

    public GetSubscriptionStatusHandlerShould()
    {
        _subscriptionRepositoryMock = new Mock<ISubscriptionRepository>();
        _planRepositoryMock = new Mock<IPlanRepository>();
        _timeProvider = new FakeTimeProvider(new DateTimeOffset(2025, 1, 1, 0, 0, 0, 0, TimeSpan.Zero));
        _handler = new GetSubscriptionStatusHandler(_subscriptionRepositoryMock.Object, _planRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnNoActiveSubscription_WhenSubscriptionNotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _subscriptionRepositoryMock
            .Setup(r => r.GetActiveSubscriptionByUserIdAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Subscription?)null);

        var request = new GetSubscriptionStatusRequest(userId);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.HasActiveSubscription.Should().BeFalse();
        result.Value.PlanId.Should().BeNull();
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenSubscriptionExistsButPlanNotFound()
    {
        // Arrange
        var user = User.Create(TelegramId.Create(123), "vpnId", UserStatus.Active);
        var plan = Plan.Monthly;
        var subscription = Subscription.Create(user, plan, _timeProvider);

        _subscriptionRepositoryMock
            .Setup(r => r.GetActiveSubscriptionByUserIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subscription);

        _planRepositoryMock
            .Setup(r => r.GetByIdAsync(plan.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Plan?)null);

        var request = new GetSubscriptionStatusRequest(user.Id);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.First().Message.Should().Be($"Plan with ID '{plan.Id}' not found for an active subscription.");
    }

    [Fact]
    public async Task Handle_Should_ReturnActiveSubscription_WhenSubscriptionAndPlanExist()
    {
        // Arrange
        var user = User.Create(TelegramId.Create(123), "vpnId", UserStatus.Active);
        var plan = Plan.Monthly;
        var startDate = _timeProvider.GetUtcNow().DateTime;
        var endDate = startDate.AddDays(plan.DurationInDays);
        var subscription = Subscription.Create(user, plan, _timeProvider);

        _subscriptionRepositoryMock
            .Setup(r => r.GetActiveSubscriptionByUserIdAsync(user.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(subscription);

        _planRepositoryMock
            .Setup(r => r.GetByIdAsync(plan.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(plan);

        var request = new GetSubscriptionStatusRequest(user.Id);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        var response = result.Value;
        response.HasActiveSubscription.Should().BeTrue();
        response.PlanId.Should().Be(plan.Id);
        response.PlanName.Should().Be(plan.Name);
        response.Status.Should().Be(subscription.Status);
        response.StartDateUtc.Should().Be(startDate);
        response.EndDateUtc.Should().Be(endDate);
    }
}