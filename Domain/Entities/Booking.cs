using System.ComponentModel.DataAnnotations.Schema;
using Stellaway.Domain.Common;
using Stellaway.Domain.Entities.Identities;
using Stellaway.Domain.Enums;

namespace Stellaway.Domain.Entities;

public class Booking : BaseAuditableEntity<Guid>
{
    public double TotalPrice { get; set; }

    [Column(TypeName = "nvarchar(24)")]
    public BookingStatus Status { get; set; }

    [Column(TypeName = "nvarchar(24)")]
    public BookingMethod Method { get; set; }

    public Guid UserId { get; set; }
    public virtual User User { get; set; } = default!;

    public int ScheduleId { get; set; }
    public virtual Schedule Schedule { get; set; } = default!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();

}
