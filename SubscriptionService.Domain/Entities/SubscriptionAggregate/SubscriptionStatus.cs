using SubscriptionService.Domain.SeedWork;

namespace SubscriptionService.Domain.Entities.SubscriptionAggregate;

public class SubscriptionStatus : Entity<int>
{
    public static readonly SubscriptionStatus Active = new SubscriptionStatus(1, nameof(Active).ToLowerInvariant());
    public static readonly SubscriptionStatus Expired = new SubscriptionStatus(2, nameof(Expired).ToLowerInvariant());
    public static readonly SubscriptionStatus Canceled = new SubscriptionStatus(3, nameof(Canceled).ToLowerInvariant());

    private SubscriptionStatus()
    {
    }

    private SubscriptionStatus(int id, string name) : this()
    {
        Id = id;
        Name = name;
    }

    public string Name { get; private set; } = null!;

    public static IEnumerable<SubscriptionStatus> GetAll()
    {
        return
        [
            Active,
            Expired,
            Canceled
        ];
    }
}