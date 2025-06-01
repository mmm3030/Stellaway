using Stellaway.Domain.Common;

namespace Stellaway.Domain.Entities;

public class Review : BaseAuditableEntity<int>
{
    public string Title { get; set; } = default!;
    public string Comment { get; set; } = default!;
    public int Rating { get; set; }

    public Guid BookingId { get; set; }
    public virtual Booking Booking { get; set; } = default!;
}
