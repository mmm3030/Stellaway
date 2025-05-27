using Stellaway.Domain.Enums;

namespace Stellaway.DTOs;

public sealed record UpdateSeatRequest
{
    public int Id { get; set; }
    public SeatStatus Status { get; set; }
    public SeatCategory Category { get; set; }
}
