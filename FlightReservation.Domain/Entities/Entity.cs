namespace FlightReservation.Domain.Entities;

public abstract class Entity<T>
{
    public T Id { get; set; } = default!;
}

// For simple audit tracking (optional)
public abstract class AuditableEntity<T> : Entity<T>
{
    public DateTime CreateDateUtc { get; set; } = DateTime.UtcNow;
    public DateTime? LastModifyDateUtc { get; set; }


    public void UpdateLastModifyDateUtc()
    {
        LastModifyDateUtc = DateTime.UtcNow;
    }
}