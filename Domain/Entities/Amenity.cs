using Stellaway.Domain.Common;

namespace Stellaway.Domain.Entities;

public class Amenity : BaseEntity<int>
{
    public string Name { get; set; } = default!;

    public virtual ICollection<RoomAmenity> RoomAmenities { get; set; } = new HashSet<RoomAmenity>();
}
