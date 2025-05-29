using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stellaway.Common.Exceptions;
using Stellaway.Common.Resources;
using Stellaway.Domain.Entities;
using Stellaway.DTOs;
using Stellaway.Repositories;

namespace Stellaway.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CheckInsController(
    IUnitOfWork unitOfWork) : ControllerBase
{

    private readonly IGenericRepository<Ticket> _ticketRepository = unitOfWork.Repository<Ticket>();
    private readonly IGenericRepository<Booking> _bookingRepository = unitOfWork.Repository<Booking>();

    [HttpPut("booking/{id}")]
    public async Task<ActionResult<MessageResponse>> UpdateCheckIn(Guid id, CancellationToken cancellationToken)
    {
        var zone = await _bookingRepository.FindByAsync(
            _ => _.Id == id,
            includeFunc: _ => _.Include(_ => _.Tickets),
            cancellationToken: cancellationToken);

        if (zone is null)
        {
            throw new NotFoundException(nameof(Booking), id);
        }

        foreach (var item in zone.Tickets)
        {
            item.IsUsed = true;
        }

        await unitOfWork.CommitAsync(cancellationToken);

        return new MessageResponse(Resource.UpdatedSuccess);
    }

    [HttpPut("ticket/{id}")]
    public async Task<ActionResult<MessageResponse>> CheckInTicket(int id, CancellationToken cancellationToken)
    {
        var zone = await _ticketRepository.FindByAsync(
            _ => _.Id == id,
            cancellationToken: cancellationToken);

        if (zone is null)
        {
            throw new NotFoundException(nameof(Ticket), id);
        }

        zone.IsUsed = true;
        await unitOfWork.CommitAsync(cancellationToken);

        return new MessageResponse(Resource.UpdatedSuccess);
    }

}
