using Stellaway.Domain.Enums;

namespace Stellaway.DTOs;

public sealed record UpdateRoomCommand
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

    public int NumberSeatsOfRow { get; set; }

    public ICollection<UpdateRoomAmenityRequest> RoomAmenities { get; set; } = new HashSet<UpdateRoomAmenityRequest>();

    public ICollection<UpdateImageRequest> RoomImages { get; set; } = new HashSet<UpdateImageRequest>();
    public ICollection<UpdateSeatRequest> Seats { get; set; } = new HashSet<UpdateSeatRequest>();
}
