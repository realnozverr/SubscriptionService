using SubscriptionService.Domain.SeedWork;

namespace SubscriptionService.Domain.Entities.PlanAggregate;

public class Plan : Entity<Guid>
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
        if (string.IsNullOrWhiteSpace(name) || durationInDays <= 0)
            throw new ArgumentOutOfRangeException("durationInDays");
        return new Plan(name, price, durationInDays);
    }
}