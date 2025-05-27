using Stellaway.Domain.Common;

namespace Stellaway.Domain.Entities;

public class RoomImage : BaseEntity<int>
{
    public string ImageUrl { get; set; } = default!;

    public int RoomId { get; set; }
    public virtual Room Room { get; set; } = default!;
}
