namespace Stellaway.Domain.Entities;

public class RoomAmenity
{

    public int RoomId { get; set; }
    public virtual Room Room { get; set; } = default!;

    public int AmenityId { get; set; }
    public virtual Amenity Amenity { get; set; } = default!;
}
