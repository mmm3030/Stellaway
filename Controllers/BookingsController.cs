﻿using Microsoft.AspNetCore.Mvc;
using Stellaway.Common.Exceptions;
using Stellaway.Domain.Entities;
using Stellaway.DTOs;
using Stellaway.DTOs.Pages;
using Stellaway.Repositories;

namespace Stellaway.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BookingsController(
    IUnitOfWork unitOfWork) : ControllerBase
{

    private readonly IGenericRepository<Booking> _bookingRepository = unitOfWork.Repository<Booking>();
    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<BookingResponse>>> GetBookings(
        [FromQuery] GetBookingsQuery request,
        CancellationToken cancellationToken)
    {
        var zones = await _bookingRepository
            .FindAsync<BookingResponse>(
                request.PageIndex,
                request.PageSize,
                request.GetExpressions(),
                request.GetOrder(),
                cancellationToken);

        return await zones.ToPaginatedResponseAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BookingResponse>> GetBookingById(Guid id, CancellationToken cancellationToken)
    {
        var zone = await _bookingRepository
            .FindByAsync<BookingResponse>(
            _ => _.Id == id,
            cancellationToken);

        if (zone is null)
        {
            throw new NotFoundException(nameof(Booking), id);
        }

        return zone;
    }

}
