namespace Stellaway.DTOs;

public sealed record CreateScheduleCommand
{
    public DateTimeOffset StartTime { get; set; }
    public double PriceVip { get; set; }
    public double PriceNormal { get; set; }
    public double PriceEconomy { get; set; }
    public int EventId { get; set; }
    public int RoomId { get; set; }
}
