using Mapster;
using Microsoft.AspNetCore.Mvc;
using Stellaway.Common.Exceptions;
using Stellaway.Common.Resources;
using Stellaway.Domain.Entities;
using Stellaway.DTOs;
using Stellaway.DTOs.Pages;
using Stellaway.Repositories;

namespace Stellaway.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SchedulesController(
    IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IGenericRepository<Schedule> _scheduleRepository = unitOfWork.Repository<Schedule>();

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<ScheduleResponse>>> GetSchedules(
        [FromQuery] GetSchedulesQuery request,
        CancellationToken cancellationToken)
    {
        var zones = await _scheduleRepository
            .FindAsync<ScheduleResponse>(
                request.PageIndex,
                request.PageSize,
                request.GetExpressions(),
                request.GetOrder(),
                cancellationToken);

        return await zones.ToPaginatedResponseAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ScheduleResponse>> GetScheduleById(int id, CancellationToken cancellationToken)
    {
        var zone = await _scheduleRepository
            .FindByAsync<ScheduleResponse>(
            _ => _.Id == id,
            cancellationToken);

        if (zone is null)
        {
            throw new NotFoundException(nameof(Schedule), id);
        }

        return zone;
    }

    [HttpPost]
    public async Task<ActionResult<MessageResponse>> CreateSchedule(CreateScheduleCommand command, CancellationToken cancellationToken)
    {

        var zone = command.Adapt<Schedule>();
        await _scheduleRepository.CreateAsync(zone, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new MessageResponse(Resource.CreatedSuccess);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MessageResponse>> UpdateSchedule(int id, UpdateScheduleCommand command, CancellationToken cancellationToken)
    {
        var zone = await _scheduleRepository.FindByAsync(
            _ => _.Id == id,
            cancellationToken: cancellationToken);

        if (zone is null)
        {
            throw new NotFoundException(nameof(Schedule), id);
        }

        command.Adapt(zone);
        await unitOfWork.CommitAsync(cancellationToken);

        return new MessageResponse(Resource.UpdatedSuccess);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<MessageResponse>> DeleteSchedule(int id, CancellationToken cancellationToken)
    {
        var zone = await _scheduleRepository.FindByAsync(
            _ => _.Id == id,
            cancellationToken: cancellationToken);

        if (zone is null)
        {
            throw new NotFoundException(nameof(Schedule), id);
        }

        await _scheduleRepository.DeleteAsync(zone);
        await unitOfWork.CommitAsync(cancellationToken);
        return new MessageResponse(Resource.DeletedSuccess);
    }
}
