using SubscriptionService.Domain.SeedWork;
using SubscriptionService.Domain.SeedWork.Exceptions.ValueExceptions;

namespace SubscriptionService.Domain.Entities.PlanAggregate;

public class Plan : Aggregate<int>
{
    private Plan()
    {
    }

    private Plan(int id, string name, decimal price, int durationInDays) : this()
    {
        Id = id;
        Name = name;
        Price = price;
        DurationInDays = durationInDays;
    }

    public static readonly Plan Monthly = Plan.Create(1,nameof(Monthly), 150m, 30);
    public static readonly Plan SemiYear = Plan.Create(2, nameof(SemiYear), 150m, 183);
    public static readonly Plan Year = Plan.Create(3, nameof(Year), 150m, 365);
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public int DurationInDays { get; private set; }

    public static Plan Create(int id, string name, decimal price, int durationInDays)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ValueIsRequiredException($"{nameof(name)} name cannot be empty.");
        
        if (price <= 0)
            throw new ValueOutOfRangeException($"{nameof(price)} cannot be negative.");
        
        if (durationInDays <= 0)
            throw new ValueOutOfRangeException($"{nameof(durationInDays)} must be a positive number of days.");
        
        return new Plan(id, name, price, durationInDays);
    }

    public static IEnumerable<Plan> GetAll()
    {
        return
        [
            Monthly,
            SemiYear,
            Year
        ];
    }
}