using Stellaway.Domain.Enums;

namespace Stellaway.DTOs;

public sealed record CreateSeatRequest
{
    public SeatStatus Status { get; set; }

    public SeatCategory Category { get; set; }
}
