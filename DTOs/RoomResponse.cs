using Stellaway.Domain.Enums;

namespace Stellaway.DTOs;

public sealed record RoomResponse : BaseAuditableEntityResponse<int>
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public int Capacity { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public RoomStatus Status { get; set; }
    public RoomCategory Category { get; set; }

    public ICollection<ImageResponse> RoomImages { get; set; } = new HashSet<ImageResponse>();

    public ICollection<RoomAmenityResponse> RoomAmenities { get; set; } = new HashSet<RoomAmenityResponse>();

    public ICollection<SeatResponse> Seats { get; set; } = new HashSet<SeatResponse>();

}
