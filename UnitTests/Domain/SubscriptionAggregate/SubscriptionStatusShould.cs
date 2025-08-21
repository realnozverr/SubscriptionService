using JetBrains.Annotations;
using SubscriptionService.Domain.Entities.SubscriptionAggregate;

namespace UnitTests.Domain.SubscriptionAggregate;

[TestSubject(typeof(SubscriptionStatus))]
public class SubscriptionStatusShould
{
    [Fact]
    public void GetAll_ShouldReturnAllDefinedStatuses()
    {
        //Arrange
        var expectedCount = 3;

        //Act
        var allStatuses = SubscriptionStatus.GetAll();
        
        //Assert
        Assert.NotNull(allStatuses);
        Assert.Equal(expectedCount, allStatuses.Count());
        Assert.Contains(SubscriptionStatus.Active, allStatuses);
        Assert.Contains(SubscriptionStatus.Expired, allStatuses);
        Assert.Contains(SubscriptionStatus.Canceled, allStatuses);
    }

    [Fact]
    public void Active_ShouldHaveCorrectProperties()
    {
        //Arrange
        var status = SubscriptionStatus.Active;
        
        //Assert
        Assert.Equal(status.Id, SubscriptionStatus.Active.Id);
        Assert.Equal(status.Name, SubscriptionStatus.Active.Name);
    }
    
    [Fact]
    public void Canceled_ShouldHaveCorrectProperties()
    {
        //Arrange
        var status = SubscriptionStatus.Canceled;
        
        //Assert
        Assert.Equal(status.Id, SubscriptionStatus.Canceled.Id);
        Assert.Equal(status.Name, SubscriptionStatus.Canceled.Name);
    }
    
    [Fact]
    public void Expired_ShouldHaveCorrectProperties()
    {
        //Arrange
        var status = SubscriptionStatus.Expired;
        
        //Assert
        Assert.Equal(status.Id, SubscriptionStatus.Expired.Id);
        Assert.Equal(status.Name, SubscriptionStatus.Expired.Name);
    }
}