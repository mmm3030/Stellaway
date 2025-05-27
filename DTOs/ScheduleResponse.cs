namespace Stellaway.DTOs;

public sealed record ScheduleResponse : BaseAuditableEntityResponse<int>
{
    public DateTimeOffset StartTime { get; set; }
    public double PriceVip { get; set; }
    public double PriceNormal { get; set; }
    public double PriceEconomy { get; set; }

    public int EventId { get; set; }
    public EventResponse Event { get; set; } = default!;

    public int RoomId { get; set; }
    public RoomResponse Room { get; set; } = default!;
}
