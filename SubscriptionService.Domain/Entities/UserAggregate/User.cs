using SubscriptionService.Domain.SeedWork;

namespace SubscriptionService.Domain.Entities.UserAggregate;

public class User : Aggregate<Guid>
{
    private User()
    {
    }

    private User(
        TelegramId telegramId,
        string vpnIdentifier,
        UserStatus status
    ) : this()
    {
        Id = Guid.NewGuid();
        TelegramId = telegramId;
        VpnIdentifier = vpnIdentifier;
        Status = status;
    }

    public TelegramId TelegramId { get; private set; }
    public string? VpnIdentifier { get; private set; }
    public UserStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    public static User Create(TelegramId telegramId, string? vpnIdentifier, UserStatus status)
    {
        
    }
}