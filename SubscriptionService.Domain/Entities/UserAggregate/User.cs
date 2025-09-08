using SubscriptionService.Domain.SeedWork;
using SubscriptionService.Domain.SeedWork.Exceptions.ValueExceptions;

namespace SubscriptionService.Domain.Entities.UserAggregate;

public class User : Aggregate<Guid>
{
    private User()
    {
    }

    private User(
        TelegramId telegramId,
        string? vpnIdentifier,
        string telegramName,
        UserStatus status
    ) : this()
    {
        Id = Guid.NewGuid();
        TelegramId = telegramId;
        VpnIdentifier = vpnIdentifier;
        TelegramName = telegramName;
        Status = status;
    }

    public TelegramId TelegramId { get; private set; }
    public string TelegramName { get; private set; }
    public string? VpnIdentifier { get; private set; }
    public UserStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public static User Create(TelegramId telegramId, string? vpnIdentifier, string telegramName, UserStatus status)
    {
        ValidateTelegramId(telegramId);
        ValidateUserStatus(status);
        ValidateTelegramName(telegramName);
        return new User(telegramId, vpnIdentifier, telegramName, status);
    }
    
    // TODO подумать когда валидировать
    private static void ValidateVpnIdentifier(string? vpnIdentifier)
    {
        if (string.IsNullOrWhiteSpace(vpnIdentifier))
        {
            throw new ValueIsRequiredException("VPN identifier is required.");
        }
    }

    private static void ValidateTelegramId(TelegramId telegramId)
    {
        if (telegramId == null)
        {
            throw new ValueIsNullException("TelegramId cannot be null.");
        }
    }
    
    private static void ValidateTelegramName(string telegramName)
    {
        if (string.IsNullOrWhiteSpace(telegramName))
        {
            throw new ValueIsNullException("TelegramName cannot be null.");
        }
    }

    private static void ValidateUserStatus(UserStatus userStatus)
    {
        if (userStatus is null)
        {
            throw new ValueIsNullException("UserStatus cannot be null.");
        }

        if (!UserStatus.GetAll().Contains(userStatus))
        {
            throw new ValueIsInvalidException("Invalid UserStatus.");
        }
    }

    private static void ValidateCreatedAt(DateTime createdAt)
    {
        if (createdAt > DateTime.UtcNow)
        {
            throw new ValueOutOfRangeException("CreatedAt date cannot be in the future.");
        }
    }
}