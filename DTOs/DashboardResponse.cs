namespace Stellaway.DTOs;

public sealed record DashboardResponse
{
    public RevenueResponse Revenue { get; set; } = default!;
    public TicketStatisticResponse TicketStatistic { get; set; } = default!;
    public UserStatisticResponse UserStatistic { get; set; } = default!;
    public EventStatisticResponse EventStatistic { get; set; } = default!;

}

public sealed record RevenueResponse
{
    public double CurrentMonthRevenue { get; set; } = default!;
    public double PercentageChange { get; set; } = default!;
}

public sealed record TicketStatisticResponse
{
    public int CurrentTicketCount { get; set; } = default!;
    public double PercentageChange { get; set; } = default!;
}

public sealed record UserStatisticResponse
{
    public int CurrentUserCount { get; set; }
    public int LastMonthUserCount { get; set; }
    public int Difference { get; set; }
}

public sealed record EventStatisticResponse
{
    public int CurrentEventCount { get; set; }
    public int LastMonthEventCount { get; set; }
    public int Difference { get; set; } // tăng/giảm so với tháng trước
}