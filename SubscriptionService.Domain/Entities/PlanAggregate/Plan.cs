using SubscriptionService.Domain.SeedWork;
using SubscriptionService.Domain.SeedWork.Exceptions.ValueExceptions;

namespace SubscriptionService.Domain.Entities.PlanAggregate;

public class Plan : Aggregate<Guid>
{
    private Plan()
    {
    }

    private Plan(string name, decimal price, int durationInDays) : this()
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
        DurationInDays = durationInDays;
    }

    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int DurationInDays { get; private set; }

    public static Plan Create(string name, decimal price, int durationInDays)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValueIsRequiredException($"{nameof(name)} name cannot be empty.");
        
        if (price <= 0)
            throw new ValueOutOfRangeException($"{nameof(price)} cannot be negative.");
        
        if (durationInDays <= 0)
            throw new ValueOutOfRangeException($"{nameof(durationInDays)} must be a positive number of days.");
        
        return new Plan(name, price, durationInDays);
    }
}