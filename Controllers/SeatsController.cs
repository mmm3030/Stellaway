using Microsoft.AspNetCore.Mvc;
using Stellaway.Domain.Entities;
using Stellaway.Domain.Enums;
using Stellaway.DTOs;
using Stellaway.Persistence.Data;
using Stellaway.Repositories;

namespace Stellaway.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SeatsController(
   IUnitOfWork unitOfWork,
   ApplicationDbContext context) : ControllerBase
{

    private readonly IGenericRepository<Seat> _seatRepository = unitOfWork.Repository<Seat>();

    [HttpGet("room/{roomId}/schedule/{scheduleId}")]
    public async Task<IActionResult> GetSeats(
       int roomId,
       int scheduleId,
       CancellationToken cancellationToken)
    {
        var seats = await _seatRepository
            .FindSelectAsync(
            expression: _ => _.RoomId == roomId,
            select: _ => _.Select(_ => new SeatSchedulerResponse
            {
                Id = _.Id,
                Index = _.Index,
                Status = _.Status,
                Category = _.Category,
                IsBooked = context.Tickets.Any(
                    t =>
                        t.SeatId == _.Id &&
                        t.Booking.ScheduleId == scheduleId &&
                        t.Booking.Status == BookingStatus.Completed)
            }));

        return Ok(seats);
    }

}
