namespace Stellaway.DTOs;

public sealed record CreateTicketRequest
{
    public double Price { get; set; }
    public string QrCode { get; set; } = default!;
    public bool IsUsed { get; set; }
    public int SeatId { get; set; }
}
