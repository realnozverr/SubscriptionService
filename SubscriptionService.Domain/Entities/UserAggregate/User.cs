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
        UserStatus status,
        Guid currentSubscription
    ) : this()
    {
        Id = Guid.NewGuid();
        TelegramId = telegramId;
        VpnIdentifier = vpnIdentifier;
        Status = status;
        CurrentSubscription = currentSubscription;
    }

    public TelegramId TelegramId { get; protected set; }
    public string? VpnIdentifier { get; protected set; }
    public UserStatus Status { get; protected set; }
    public Guid CurrentSubscription { get; protected set; }
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
}