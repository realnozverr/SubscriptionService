using JetBrains.Annotations;
using SubscriptionService.Domain.Entities.UserAggregate;

namespace UnitTests.Domain.UserAggregate;

[TestSubject(typeof(UserStatus))]
public class UserStatusShould
{
    [Fact]
    public void GetAll_ShouldReturnAllDefinedStatuses()
    {
        //Arrange
        var expectedCount = 3;
        
        //Act
        var allStatuses = UserStatus.GetAll();
        
        //Assert
        Assert.NotNull(allStatuses);
        Assert.Equal(expectedCount, allStatuses.Count());
        Assert.Contains(UserStatus.Active, allStatuses);
        Assert.Contains(UserStatus.Expired, allStatuses);
        Assert.Contains(UserStatus.Banned, allStatuses);
    }

    [Fact]
    public void Active_Status_ShouldHaveCorrectValues()
    {
        //Arrange
        var activeStatus = UserStatus.Active;
        
        //Assert
        Assert.Equal(activeStatus.Id, UserStatus.Active.Id);
        Assert.Equal(activeStatus.Name, UserStatus.Active.Name);
    }

    [Fact]
    public void Expired_Status_ShouldHaveCorrectValues()
    {
        //Arrange
        var expiredStatus = UserStatus.Expired;
        
        //Assert
        Assert.Equal(expiredStatus.Id, UserStatus.Expired.Id);
        Assert.Equal(expiredStatus.Name, UserStatus.Expired.Name);
    }

    [Fact]
    public void Banned_Status_ShouldHaveCorrectValues()
    {
        //Arrange
        var bannedStatus = UserStatus.Banned;
        
        //Assert
        Assert.Equal(bannedStatus.Id, UserStatus.Banned.Id);
        Assert.Equal(bannedStatus.Name, UserStatus.Banned.Name);
    }
    
    
}