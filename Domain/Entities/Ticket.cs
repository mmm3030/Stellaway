using Stellaway.Domain.Common;

namespace Stellaway.Domain.Entities;

public class Ticket : BaseAuditableEntity<int>
{
    public double Price { get; set; }
    public string QrCode { get; set; } = default!;
    public bool IsUsed { get; set; }

    public Guid BookingId { get; set; }
    public virtual Booking Booking { get; set; } = default!;

    public int SeatId { get; set; }
    public virtual Seat Seat { get; set; } = default!;

}
