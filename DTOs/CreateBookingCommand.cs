using Stellaway.Domain.Enums;

namespace Stellaway.DTOs;

public sealed record CreateBookingCommand
{
    public double TotalPrice { get; set; }

    public BookingMethod Method { get; set; }

    public Guid UserId { get; set; }

    public int ScheduleId { get; set; }

    public ICollection<CreateTicketRequest> Tickets { get; set; } = new HashSet<CreateTicketRequest>();
}
