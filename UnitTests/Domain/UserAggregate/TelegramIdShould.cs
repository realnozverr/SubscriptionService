using JetBrains.Annotations;
using SubscriptionService.Domain.Entities.UserAggregate;
using SubscriptionService.Domain.SeedWork.Exceptions.ValueExceptions;

namespace UnitTests.Domain.UserAggregate;

[TestSubject(typeof(TelegramId))]
public class TelegramIdShould
{
    [Fact]
    public void Create_WithPositiveValue_ShouldCreateInstanceSuccessfully()
    {
        //Arrange
        long validId = 123456789;
        
        //Act
        var telegramId = TelegramId.Create(validId);
        
        //Assert
        Assert.NotNull(telegramId);
        Assert.Equal(validId, telegramId.Value);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-123)]
    public void Create_WithZeroOrNegativeValue_ShouldThrowValueOutOfRangeException(long invalidId)
    {
        //Arrange
        Action act = () => TelegramId.Create(invalidId);
        
        //Assert
        Assert.Throws<ValueOutOfRangeException>(act);
    }

    [Fact]
    public void Equality_WhenValuesAreSame_ShouldBeEqual()
    {
        //Arrange
        long value = 12345;
        var telegramId1 = TelegramId.Create(value);
        var telegramId2 = TelegramId.Create(value);
        
        //Assert
        Assert.Equal(telegramId1, telegramId2);
        Assert.True(telegramId1 == telegramId2);
    }

    [Fact]
    public void Equality_WhenValuesAreDifferent_ShouldNotBeEqual()
    {
        //Arrange
        var telegramId1 = TelegramId.Create(123);
        var telegramId2 = TelegramId.Create(456);
        
        //Assert
        Assert.NotEqual(telegramId1, telegramId2);
        Assert.True(telegramId1 != telegramId2);
    }

    [Fact]
    public void ToString_ShouldReturnValueAsString()
    {
        //Arrange
        long value = 98765;
        var telegramId = TelegramId.Create(value);
        
        //Act
        var stringRepresentation = telegramId.ToString();
        
        //Assert
        Assert.Equal(value.ToString(), stringRepresentation);
    }
}