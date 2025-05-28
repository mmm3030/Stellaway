using Microsoft.AspNetCore.Mvc;
using Stellaway.Common.Exceptions;
using Stellaway.Domain.Entities;
using Stellaway.DTOs;
using Stellaway.DTOs.Pages;
using Stellaway.Repositories;

namespace Stellaway.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TicketsController(
    IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IGenericRepository<Ticket> _zoneRepository = unitOfWork.Repository<Ticket>();

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<TicketResponse>>> GetTickets(
        [FromQuery] GetTicketsQuery request,
        CancellationToken cancellationToken)
    {
        var zones = await _zoneRepository
            .FindAsync<TicketResponse>(
                request.PageIndex,
                request.PageSize,
                request.GetExpressions(),
                request.GetOrder(),
                cancellationToken);

        return await zones.ToPaginatedResponseAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TicketResponse>> GetTicketById(int id, CancellationToken cancellationToken)
    {
        var zone = await _zoneRepository
            .FindByAsync<TicketResponse>(
            _ => _.Id == id,
            cancellationToken);

        if (zone is null)
        {
            throw new NotFoundException(nameof(Ticket), id);
        }

        return zone;
    }

}
