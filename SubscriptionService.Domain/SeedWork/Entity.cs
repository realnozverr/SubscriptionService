namespace SubscriptionService.Domain.SeedWork;

public abstract class Entity<TId> : IComparable, IComparable<Entity<TId>>
    where TId : IComparable<TId>
{
    public virtual TId? Id { get; protected set; }

    protected Entity()
    {
    }

    protected Entity(TId id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (!(obj is Entity<TId> other))
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (obj.GetType() != GetType())
            return false;

        if (IsTransient() || other.IsTransient())
            return false;

        return Id!.Equals(other.Id);
    }

    private bool IsTransient()
    {
        return Id is null || Id.Equals(default(TId));
    }

    public static bool operator ==(Entity<TId> a, Entity<TId> b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entity<TId> a, Entity<TId> b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return Id?.GetHashCode() ?? 0;
    }

    public virtual int CompareTo(Entity<TId>? other)
    {
        if (other is null)
            return 1;

        if (ReferenceEquals(this, other))
            return 0;

        return Id!.CompareTo(other.Id);
    }

    public int CompareTo(object? other)
    {
        return CompareTo(other as Entity<TId>);
    }
}