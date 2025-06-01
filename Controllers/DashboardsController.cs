using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stellaway.Domain.Entities;
using Stellaway.Domain.Entities.Identities;
using Stellaway.Domain.Enums;
using Stellaway.DTOs;
using Stellaway.Persistence.Data;
using Stellaway.Repositories;

namespace Stellaway.Controllers;
[Route("api/[controller]")]
[ApiController]
public class DashboardsController(
    ApplicationDbContext _context,
    IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IGenericRepository<Booking> _bookingRepository = unitOfWork.Repository<Booking>();
    private readonly IGenericRepository<Event> _eventRepository = unitOfWork.Repository<Event>();
    private readonly IGenericRepository<Ticket> _zoneRepository = unitOfWork.Repository<Ticket>();
    private readonly IGenericRepository<User> _userRepository = unitOfWork.Repository<User>();

    [HttpGet()]
    public async Task<IActionResult> GetDashboard()
    {
        var now = DateTime.Now;
        var firstDayThisMonth = new DateTime(now.Year, now.Month, 1);
        var firstDayLastMonth = firstDayThisMonth.AddMonths(-1);
        var firstDayNextMonth = firstDayThisMonth.AddMonths(1);
        var firstDayThisMonthForLastCheck = firstDayThisMonth;
        int currentYear = DateTimeOffset.UtcNow.Year;

        var currentMonthRevenue = (await _bookingRepository.FindAsync(b => b.CreatedAt >= firstDayThisMonth && b.CreatedAt < firstDayNextMonth))
            .Sum(b => b.TotalPrice);

        var lastMonthRevenue = (await _bookingRepository.FindAsync(b => b.CreatedAt >= firstDayLastMonth && b.CreatedAt < firstDayThisMonth))
                .Sum(b => b.TotalPrice);

        double percentChange = 0;

        if (lastMonthRevenue > 0)
        {
            percentChange = (double)(currentMonthRevenue - lastMonthRevenue) / (double)lastMonthRevenue * 100;
        }
        var revenue = new RevenueResponse
        {
            CurrentMonthRevenue = currentMonthRevenue,
            PercentageChange = percentChange

        };

        // Vé đã bán trong tháng này
        var currentCount = await _context.Tickets
            .Where(t => (t.CreatedAt >= firstDayThisMonth && t.CreatedAt < firstDayNextMonth) && t.Booking.Status == BookingStatus.Completed)
            .CountAsync();

        // Vé đã bán trong tháng trước
        var lastMonthCount = await _context.Tickets
            .Where(t => (t.CreatedAt >= firstDayLastMonth && t.CreatedAt < firstDayThisMonthForLastCheck) && t.Booking.Status == BookingStatus.Completed)
            .CountAsync();

        double percentTicketChange = 0;

        if (lastMonthCount > 0)
        {
            percentTicketChange = ((double)(currentCount - lastMonthCount) / lastMonthCount) * 100;
        }
        else if (currentCount > 0)
        {
            percentTicketChange = 100;
        }

        var ticketStatistic = new TicketStatisticResponse
        {
            CurrentTicketCount = currentCount,
            PercentageChange = percentTicketChange

        };

        // Người dùng mới trong tháng này
        var currentUserCount = await _context.Users
            .Where(u => u.CreatedAt >= firstDayThisMonth && u.CreatedAt < firstDayNextMonth)
            .CountAsync();

        // Người dùng mới trong tháng trước
        var lastMonthUserCount = await _context.Users
            .Where(u => u.CreatedAt >= firstDayLastMonth && u.CreatedAt < firstDayThisMonth)
            .CountAsync();

        int difference = currentUserCount - lastMonthUserCount;

        var userStatistic = new UserStatisticResponse
        {
            CurrentUserCount = currentUserCount,
            Difference = difference,
            LastMonthUserCount = lastMonthUserCount

        };

        // Sự kiện đang diễn ra hiện tại (tháng này)
        var currentEventCount = await _context.Events
            .Where(e =>
                e.Schedules.Any(e =>
            e.StartTime <= now &&
            e.StartTime >= firstDayThisMonth &&
            e.StartTime < firstDayNextMonth)
            )
            .CountAsync();

        // Sự kiện đã diễn ra tháng trước
        var lastMonthEventCount = await _context.Events
            .Where(e => e.Schedules.Any(e =>
                e.StartTime >= firstDayLastMonth &&
                e.StartTime < firstDayThisMonthForLastCheck)

            )
            .CountAsync();

        var eventStatistic = new EventStatisticResponse
        {
            CurrentEventCount = currentEventCount,
            LastMonthEventCount = lastMonthEventCount,
            Difference = currentEventCount - lastMonthEventCount
        };

        var revenueInYear = await _context.Bookings
          .Where(t => t.CreatedAt!.Value.Year == currentYear)
          .GroupBy(t => t.CreatedAt!.Value.Month)
          .Select(g => new MonthlyRevenue
          {
              Month = g.Key,
              Total = g.Sum(t => t.TotalPrice)
          })
          .ToListAsync();

        // Đảm bảo có đủ 12 tháng (có thể có tháng không có dữ liệu)
        var result = Enumerable.Range(1, 12)
            .Select(m => new MonthlyRevenue
            {
                Month = m,
                Total = revenueInYear.FirstOrDefault(x => x.Month == m)?.Total ?? 0
            })
            .ToList();

        return Ok(new DashboardResponse
        {
            Revenue = revenue,
            TicketStatistic = ticketStatistic,
            UserStatistic = userStatistic,
            EventStatistic = eventStatistic,
            MonthlyRevenues = result

        });
    }

}
