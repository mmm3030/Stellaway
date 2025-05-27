using System.ComponentModel.DataAnnotations.Schema;
using Stellaway.Domain.Common;
using Stellaway.Domain.Enums;

namespace Stellaway.Domain.Entities;

public class Seat : BaseEntity<int>
{
    [Column(TypeName = "nvarchar(24)")]
    public SeatStatus Status { get; set; }

    [Column(TypeName = "nvarchar(24)")]
    public SeatCategory Category { get; set; }

    public int RoomId { get; set; }
    public virtual Room Room { get; set; } = default!;
}
