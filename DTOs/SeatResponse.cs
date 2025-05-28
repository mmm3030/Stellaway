using Stellaway.Domain.Enums;

namespace Stellaway.DTOs;

public sealed record SeatResponse : BaseEntityResponse<int>
{
    public int Index { get; set; }
    public SeatStatus Status { get; set; }
    public SeatCategory Category { get; set; }

}
