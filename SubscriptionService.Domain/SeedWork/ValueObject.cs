namespace SubscriptionService.Domain.SeedWork;

public abstract class ValueObject : IEquatable<ValueObject>
{
    private int? _cachedHashCode;
    protected abstract IEnumerable<object> GetEqualityComponents();

    public bool Equals(ValueObject? other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (GetType() != other.GetType())
            return false;

        return GetEqualityComponents()
            .SequenceEqual(other.GetEqualityComponents());
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as ValueObject);
    }

    public override int GetHashCode()
    {
        if (!_cachedHashCode.HasValue)
        {
            var hash = new HashCode();
            foreach (var component in GetEqualityComponents())
            {
                hash.Add(component?.GetHashCode() ?? 0);
            }

            _cachedHashCode = hash.ToHashCode();
        }

        return _cachedHashCode.Value;
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !(left == right);
    }
}