namespace Stellaway.DTOs;

public sealed record RoomAmenityResponse
{
    public int AmenityId { get; set; }
    public AmenityResponse Amenity { get; set; } = default!;
}
