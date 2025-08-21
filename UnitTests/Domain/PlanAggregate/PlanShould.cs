using JetBrains.Annotations;
using SubscriptionService.Domain.Entities.PlanAggregate;
using SubscriptionService.Domain.SeedWork.Exceptions.ValueExceptions;

namespace UnitTests.Domain.PlanAggregate;

[TestSubject(typeof(Plan))]
public class PlanShould
{
    [Fact]
    public void Create_WithValidParameters_ShouldReturnPlanWithCorrectValues()
    {
        //Arrange
        var id = 10;
        var name = "Test Plan";
        var price = 100m;
        var duration = 30;

        //Act
        var plan = Plan.Create(id, name, price, duration);

        //Assert
        Assert.NotNull(plan);
        Assert.Equal(id, plan.Id);
        Assert.Equal(name, plan.Name);
        Assert.Equal(price, plan.Price);
        Assert.Equal(duration, plan.DurationInDays);
    }

    [Theory]
    [InlineData(-100)]
    [InlineData(0)] // Добавляем проверку на граничное значение (ноль)
    public void Create_WhenPriceIsInvalid_ShouldThrowValueOutOfRangeException(decimal invalidPrice)
    {
        //Arrange
        var id = 10;
        var name = "Test Plan";
        var duration = 30;

        //Act
        Action act = () => Plan.Create(id, name, invalidPrice, duration);

        //Assert
        Assert.Throws<ValueOutOfRangeException>(act);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Create_WhenNameIsInvalid_ShouldThrowValueIsRequiredException(string? name)
    {
        //Arrange
        var id = 10;
        var price = 250m;
        var duration = 30;

        //Act
        Action act = () => Plan.Create(id, name, price, duration);

        //Assert
        Assert.Throws<ValueIsRequiredException>(act);
    }
    
    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Create_WhenDurationIsInvalid_ShouldThrowValueOutOfRangeException(int invalidDuration)
    {
        //Arrange
        var id = 10;
        var name = "Test Plan";
        var price = 100m;

        //Act
        Action act = () => Plan.Create(id, name, price, invalidDuration);

        //Assert
        Assert.Throws<ValueOutOfRangeException>(act);
    }
}