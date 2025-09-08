using SubscriptionService.Domain.Entities.PlanAggregate;
using SubscriptionService.Domain.Entities.SubscriptionAggregate.DomainEvents;
using SubscriptionService.Domain.Entities.UserAggregate;
using SubscriptionService.Domain.SeedWork;
using SubscriptionService.Domain.SeedWork.Exceptions.ValueExceptions;
using SubscriptionService.Domain.SeedWork.Exceptions.ViolationExceptions;

namespace SubscriptionService.Domain.Entities.SubscriptionAggregate;

public class Subscription : Aggregate<Guid>
{
    private static readonly TimeSpan DefaultSubscriptionTime = TimeSpan.FromDays(30);

    private Subscription()
    {
    }

    private Subscription(
        Guid userId,
        int planId,
        SubscriptionStatus status,
        DateTime startDate,
        DateTime endDate
    ) : this()
    {
        Id = Guid.NewGuid();
        UserId = userId;
        PlanId = planId;
        Status = status;
        StartDate = startDate;
        EndDate = endDate;
    }

    public Guid UserId { get; private set; }
    public int PlanId { get; private set; }
    public SubscriptionStatus Status { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public void Expire()
    {
        if (Status != SubscriptionStatus.Active)
            throw new InvariantViolationException("Only active subscriptions can be canceled");
        Status = SubscriptionStatus.Expired;
        AddDomainEvent(new SubscriptionExpiredEvent(Id, UserId));
    }

    public void Cancel()
    {
        if (Status != SubscriptionStatus.Active)
            throw new InvariantViolationException("Only active subscriptions can be canceled");
        Status = SubscriptionStatus.Canceled;
        AddDomainEvent(new SubscriptionCanceledEvent(Id, UserId));
    }

    public static Subscription Create(User user, Plan plan, TimeProvider timeProvider)
    {
        if (user is null)
            throw new ValueIsNullException($"{nameof(user)} cannot be null");
        if (plan is null)
            throw new ValueIsNullException($"{nameof(plan)} cannot be null");

        var startDateTime = timeProvider.GetUtcNow().UtcDateTime;
        var endDateTime = timeProvider.GetUtcNow().Add(TimeSpan.FromDays(plan.DurationInDays)).UtcDateTime;

        var subscription = new Subscription(user.Id, plan.Id, SubscriptionStatus.Active, startDateTime, endDateTime);

        subscription.AddDomainEvent(new SubscriptionCreatedEvent(subscription.Id, user.Id, plan.Id, user.TelegramName, endDateTime));

        return subscription;
    }
}