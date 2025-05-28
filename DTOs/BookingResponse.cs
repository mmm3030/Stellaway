using Stellaway.Domain.Enums;

namespace Stellaway.DTOs;

public sealed record BookingResponse : BaseAuditableEntityResponse<Guid>
{
    public double TotalPrice { get; set; }
    public BookingStatus Status { get; set; }
    public BookingMethod Method { get; set; }

    public Guid UserId { get; set; }
    public UserResponse User { get; set; } = default!;

    public int ScheduleId { get; set; }
    public ScheduleResponse Schedule { get; set; } = default!;

    public ICollection<TicketResponse> Tickets { get; set; } = new HashSet<TicketResponse>();
}
