using Stellaway.Domain.Enums;

namespace Stellaway.DTOs;

public sealed record CreateRoomCommand
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

    public ICollection<CreateRoomAmenityRequest> RoomAmenities { get; set; } = new HashSet<CreateRoomAmenityRequest>();

    public ICollection<CreateImageRequest> RoomImages { get; set; } = new HashSet<CreateImageRequest>();
    public ICollection<CreateSeatRequest> Seats { get; set; } = new HashSet<CreateSeatRequest>();

}
