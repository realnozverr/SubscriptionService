using SubscriptionService.Domain.Entities.SubscriptionAggregate.DomainEvents;
using SubscriptionService.Domain.SeedWork;

namespace SubscriptionService.Domain.Entities.SubscriptionAggregate;

public class Subscription : Aggregate<Guid>
{
    private Subscription()
    {
    }

    private Subscription(
        Guid userId,
        Guid planId,
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
    public Guid PlanId { get; private set; } 
    public SubscriptionStatus Status { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }

    public void Expire()
    {
        if (Status == SubscriptionStatus.Active)
        {
            Status = SubscriptionStatus.Expired;
            AddDomainEvent(new SubscriptionExpiredEvent(Id, UserId));
        }
    }

    public void Cancel()
    {
        if (Status == SubscriptionStatus.Active)
        {
            Status = SubscriptionStatus.Canceled;
            AddDomainEvent(new SubscriptionCanceledEvent(Id, UserId));
        }
    }
}