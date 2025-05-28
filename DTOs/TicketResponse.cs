namespace Stellaway.DTOs;

public sealed record TicketResponse : BaseAuditableEntityResponse<int>
{
    public double Price { get; set; }
    public string QrCode { get; set; } = default!;
    public bool IsUsed { get; set; }

    public Guid BookingId { get; set; }

    public int SeatId { get; set; }
    public SeatResponse Seat { get; set; } = default!;
}
