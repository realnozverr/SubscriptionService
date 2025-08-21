using JetBrains.Annotations;
using Microsoft.Extensions.Time.Testing;
using SubscriptionService.Domain.Entities.PlanAggregate;
using SubscriptionService.Domain.Entities.SubscriptionAggregate;
using SubscriptionService.Domain.Entities.SubscriptionAggregate.DomainEvents;
using SubscriptionService.Domain.Entities.UserAggregate;
using SubscriptionService.Domain.SeedWork.Exceptions.ValueExceptions;
using SubscriptionService.Domain.SeedWork.Exceptions.ViolationExceptions;

namespace UnitTests.Domain.SubscriptionAggregate;

[TestSubject(typeof(Subscription))]
public class SubscriptionShould
{
    private readonly User _testUser;
    private readonly Plan _testPlan;
    private readonly FakeTimeProvider _timeProvider;
    public SubscriptionShould()
    {
       _testUser = User.Create(TelegramId.Create(123), "vpn_id", UserStatus.Active);
       _testPlan = Plan.Create(99, "Test plan", 100m, 30);
       _timeProvider = new FakeTimeProvider(new DateTimeOffset(2025, 1, 1,0,0,0,0, TimeSpan.Zero));
    }

    [Fact]
    public void Create_WithValidParameters_ShouldCreateActiveSubscription()
    {
        //Act
        var subscription = Subscription.Create(_testUser, _testPlan, _timeProvider);
        
        //Assert
        Assert.NotNull(subscription);
        Assert.Equal(_testUser.Id, subscription.UserId);
        Assert.Equal(_testPlan.Id, subscription.PlanId);
        Assert.Equal(SubscriptionStatus.Active, subscription.Status);
        Assert.Equal(_timeProvider.GetUtcNow().UtcDateTime, subscription.StartDate);
        Assert.Equal(_timeProvider.GetUtcNow().AddDays(_testPlan.DurationInDays).UtcDateTime, subscription.EndDate);

        Assert.Single(subscription.DomainEvents);
        Assert.IsType<SubscriptionCreatedEvent>(subscription.DomainEvents.First());
    }

    [Fact]
    public void Create_WithNullUser_ShouldThrowValueIsNullException()
    {
        //Act
        Action act = () => Subscription.Create(null, _testPlan, _timeProvider);
        
        //Assert
        Assert.Throws<ValueIsNullException>(act);
    }

    [Fact]
    public void Create_WithNullPlan_ShouldThrowValueIsNullException()
    {
        //Act
        Action act = () => Subscription.Create(_testUser, null, _timeProvider);
        
        //Assert
        Assert.Throws<ValueIsNullException>(act);
    }

    [Fact]
    public void Expire_WhenActive_ShouldSetStatusToExpiredAndAddEvent()
    {
        //Arrange
        var subscription = Subscription.Create(_testUser, _testPlan, _timeProvider);
        subscription.ClearDomainEvents();
        
        //Act
        subscription.Expire();
        
        //Assert
        Assert.Equal(SubscriptionStatus.Expired, subscription.Status);
        Assert.Single(subscription.DomainEvents);
        Assert.IsType<SubscriptionExpiredEvent>(subscription.DomainEvents.First());
    }

    [Fact]
    public void Cancel_WhenActive_ShouldSetStatusToCanceledAndAddEvent()
    {
        //Arrange 
        var subscription = Subscription.Create(_testUser, _testPlan, _timeProvider);
        subscription.ClearDomainEvents();
        
        //Act
        subscription.Cancel();
        
        //Assert
        Assert.Equal(SubscriptionStatus.Canceled, subscription.Status);
        Assert.Single(subscription.DomainEvents);
        Assert.IsType<SubscriptionCanceledEvent>(subscription.DomainEvents.First());
    }
    
    [Fact]
    public void Expire_WhenNotActive_ShouldThrowInvariantViolationException()
    {
        //Arrange
        var subscription = Subscription.Create(_testUser, _testPlan, _timeProvider);
        subscription.Expire();
        
        //Act & Assert
        Assert.Throws<InvariantViolationException>(() => subscription.Expire());
    }
}