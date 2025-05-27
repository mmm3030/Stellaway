using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stellaway.Common.Exceptions;
using Stellaway.Common.Resources;
using Stellaway.Domain.Entities;
using Stellaway.DTOs;
using Stellaway.DTOs.Pages;
using Stellaway.Repositories;

namespace Stellaway.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EventsController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IGenericRepository<Event> _eventRepository = unitOfWork.Repository<Event>();

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<EventResponse>>> GetEvents(
        [FromQuery] GetEventsQuery request,
        CancellationToken cancellationToken)
    {
        var zones = await _eventRepository
             .FindAsync<EventResponse>(
                 request.PageIndex,
                 request.PageSize,
                 request.GetExpressions(),
                 request.GetOrder(),
                 cancellationToken);

        return await zones.ToPaginatedResponseAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EventResponse>> GetEventById(int id, CancellationToken cancellationToken)
    {
        var zone = await _eventRepository
               .FindByAsync<EventResponse>(
               _ => _.Id == id,
               cancellationToken);

        if (zone is null)
        {
            throw new NotFoundException(nameof(Event), id);
        }

        return zone;
    }

    [HttpPost]
    public async Task<ActionResult<MessageResponse>> CreateEvent(CreateEventCommand command, CancellationToken cancellationToken)
    {

        var zone = command.Adapt<Event>();
        await _eventRepository.CreateAsync(zone, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new MessageResponse(Resource.CreatedSuccess);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MessageResponse>> UpdateEvent(int id, UpdateEventCommand command, CancellationToken cancellationToken)
    {
        var zone = await _eventRepository.FindByAsync(
             _ => _.Id == id,
             includeFunc: q => q.Include(_ => _.EventImages),
             cancellationToken: cancellationToken);

        if (zone is null)
        {
            throw new NotFoundException(nameof(Event), id);
        }

        command.Adapt(zone);
        await unitOfWork.CommitAsync(cancellationToken);

        return new MessageResponse(Resource.UpdatedSuccess);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<MessageResponse>> DeleteEvent(int id, CancellationToken cancellationToken)
    {
        var zone = await _eventRepository.FindByAsync(
             _ => _.Id == id,
             cancellationToken: cancellationToken);

        if (zone is null)
        {
            throw new NotFoundException(nameof(Event), id);
        }

        await _eventRepository.DeleteAsync(zone);
        await unitOfWork.CommitAsync(cancellationToken);
        return new MessageResponse(Resource.DeletedSuccess);
    }

}
