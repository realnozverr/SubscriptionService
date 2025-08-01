using SubscriptionService.Domain.SeedWork;

namespace SubscriptionService.Domain.Entities.UserAggregate;

public class UserStatus : Entity<int>
{
    public static readonly UserStatus Active = new UserStatus(1, nameof(Active).ToLowerInvariant());
    public static readonly UserStatus Expired = new UserStatus(2, nameof(Expired).ToLowerInvariant());
    public static readonly UserStatus Banned = new UserStatus(3, nameof(Banned).ToLowerInvariant());

    private UserStatus()
    {
    }

    private UserStatus(int id, string name) : this()
    {
        Id = id;
        Name = name;
    }

    public string Name { get; } = null!;

    public static IEnumerable<UserStatus> GetAll()
    {
        return
        [
            Active,
            Expired,
            Banned
        ];
    }
}