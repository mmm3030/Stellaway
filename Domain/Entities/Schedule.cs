using Stellaway.Domain.Common;

namespace Stellaway.Domain.Entities;

public class Schedule : BaseAuditableEntity<int>
{
    public DateTimeOffset StartTime { get; set; }
    public double PriceVip { get; set; }
    public double PriceNormal { get; set; }
    public double PriceEconomy { get; set; }

    public int EventId { get; set; }
    public virtual Event Event { get; set; } = default!;

    public int RoomId { get; set; }
    public virtual Room Room { get; set; } = default!;

    public virtual ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();

}
