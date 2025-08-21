using JetBrains.Annotations;
using SubscriptionService.Domain.Entities.UserAggregate;
using SubscriptionService.Domain.SeedWork.Exceptions.ValueExceptions;

namespace UnitTests.Domain.UserAggregate;

[TestSubject(typeof(User))]
public class UserShould
{
    [Fact]
    public void Create_WithValidParameters_ShouldCreateUser()
    {
        // Arrange
        var telegramId = TelegramId.Create(12345);
        var userStatus = UserStatus.Active;
        var vpnIdentifier = "test_identifier";

        // Act
        var user = User.Create(telegramId, vpnIdentifier, userStatus);

        // Assert
        Assert.NotNull(user);
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal(telegramId, user.TelegramId);
        Assert.Equal(vpnIdentifier, user.VpnIdentifier);
        Assert.Equal(userStatus, user.Status);
        Assert.True(user.CreatedAt > DateTime.UtcNow.AddMinutes(-1));
    }

    [Fact]
    public void Create_WithNullTelegramId_ShouldThrowValueIsNullException()
    {
        // Arrange
        var userStatus = UserStatus.Active;
        var vpnIdentifier = "test_identifier";

        // Act & Assert
        var exception = Assert.Throws<ValueIsNullException>(() => User.Create(null, vpnIdentifier, userStatus));
        Assert.Equal("TelegramId cannot be null.", exception.Message);
    }

    [Fact]
    public void Create_WithNullUserStatus_ShouldThrowValueIsNullException()
    {
        // Arrange
        var telegramId = TelegramId.Create(12345);
        var vpnIdentifier = "test_identifier";

        // Act & Assert
        var exception = Assert.Throws<ValueIsNullException>(() => User.Create(telegramId, vpnIdentifier, null));
        Assert.Equal("UserStatus cannot be null.", exception.Message);
    }

    [Fact]
    public void Create_WithNullVpnIdentifier_ShouldCreateUser()
    {
        // Arrange
        var telegramId = TelegramId.Create(12345);
        var userStatus = UserStatus.Active;

        // Act
        var user = User.Create(telegramId, null, userStatus);

        // Assert
        Assert.NotNull(user);
        Assert.Null(user.VpnIdentifier);
    }
}