using Stellaway.Domain.Enums;

namespace Stellaway.DTOs;

public sealed record SeatSchedulerResponse : BaseEntityResponse<int>
{
    public int Index { get; set; }

    public SeatStatus Status { get; set; }

    public SeatCategory Category { get; set; }

    public bool IsBooked { get; set; }

}
