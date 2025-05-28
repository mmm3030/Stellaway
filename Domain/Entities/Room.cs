using System.ComponentModel.DataAnnotations.Schema;
using Stellaway.Domain.Common;
using Stellaway.Domain.Enums;

namespace Stellaway.Domain.Entities;

public class Room : BaseAuditableEntity<int>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public int Capacity { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    [Column(TypeName = "nvarchar(24)")]
    public RoomStatus Status { get; set; }

    [Column(TypeName = "nvarchar(24)")]
    public RoomCategory Category { get; set; }

    public int NumberSeatsOfRow { get; set; }

    public virtual ICollection<RoomAmenity> RoomAmenities { get; set; } = new HashSet<RoomAmenity>();

    public virtual ICollection<RoomImage> RoomImages { get; set; } = new HashSet<RoomImage>();
    public virtual ICollection<Seat> Seats { get; set; } = new HashSet<Seat>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new HashSet<Schedule>();

}
