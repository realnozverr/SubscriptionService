using FluentAssertions;
using JetBrains.Annotations;
using Moq;
using SubscriptionService.Application.UseCases.Queries.GetListAvailablePlans;
using SubscriptionService.Domain.Abstractions.Repositories;
using SubscriptionService.Domain.Entities.PlanAggregate;

namespace UnitTests.Application.UseCases.ListAvailablePlans;

[TestSubject(typeof(GetListAvailablePlansHandler))]
public class GetListAvailablePlansHandlerShould
{
    private readonly Mock<IPlanRepository> _planRepositoryMock;
    private readonly GetListAvailablePlansHandler _handler;

    public GetListAvailablePlansHandlerShould()
    {
        _planRepositoryMock = new Mock<IPlanRepository>();
        _handler = new GetListAvailablePlansHandler(_planRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenNoPlansExist()
    {
        // Arrange
        _planRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Plan>());

        var request = new GetListAvailablePlansRequest();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsFailed.Should().BeTrue();
        result.Errors.First().Message.Should().Be("No plans found.");
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccessWithPlans_WhenPlansExist()
    {
        // Arrange
        var plans = new List<Plan> { Plan.Monthly, Plan.SemiYear, Plan.Year };
        _planRepositoryMock
            .Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(plans);

        var request = new GetListAvailablePlansRequest();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.Plans.Should().HaveCount(3);
        result.Value.Plans[0].Id.Should().Be(Plan.Monthly.Id);
        result.Value.Plans[1].Id.Should().Be(Plan.SemiYear.Id);
        result.Value.Plans[2].Id.Should().Be(Plan.Year.Id);
    }
}