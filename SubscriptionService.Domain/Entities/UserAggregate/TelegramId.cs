using SubscriptionService.Domain.SeedWork;

namespace SubscriptionService.Domain.Entities.UserAggregate;

public class TelegramId : ValueObject
{
    private TelegramId()
    {
    }

    private TelegramId(long value) : this()
    {
        Value = value;
    }

    public long Value { get; }

    public static TelegramId Create(long value)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Telegram id cannot be negative.");

        return new TelegramId(value);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}