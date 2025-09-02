using Proto.SubscriptionV1;
using SubscriptionService.Domain.Entities.SubscriptionAggregate;
using SubscriptionService.Domain.Entities.UserAggregate;
using SubscriptionService.Domain.SeedWork.Exceptions.ValueExceptions;

namespace SubscriptionService.API.Mapper;

public class StatusMapper
{
    public SubscriptionStatusProto SubscriptionStatusMap(SubscriptionStatus status)
    {
        return status switch
        {
            _ when status == SubscriptionStatus.Active => SubscriptionStatusProto.SubscriptionStatusActive,
            _ when status == SubscriptionStatus.Canceled => SubscriptionStatusProto.SubscriptionStatusCancelled,
            _ when status == SubscriptionStatus.Expired => SubscriptionStatusProto.SubscriptionStatusExpired,
            _ => throw new ValueOutOfRangeException($"{nameof(status)} is unknown subscription status")
        };
    }
    
    public UserStatusProto UserStatusMap(UserStatus status)
    {
        return status switch
        {
            _ when status == UserStatus.Active => UserStatusProto.UserStatusActive,
            _ when status == UserStatus.Banned => UserStatusProto.UserStatusBlocked,
            _ when status == UserStatus.Expired => UserStatusProto.UserStatusExpired,
            _ => throw new ValueOutOfRangeException($"{nameof(status)} is unknown user status")
        };
    }
}