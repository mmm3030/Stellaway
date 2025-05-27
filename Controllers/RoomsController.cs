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
public class RoomsController(
    IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IGenericRepository<Room> _roomRepository = unitOfWork.Repository<Room>();

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<RoomResponse>>> GetRooms(
     [FromQuery] GetRoomsQuery request,
     CancellationToken cancellationToken)
    {
        var zones = await _roomRepository
            .FindAsync<RoomResponse>(
                request.PageIndex,
                request.PageSize,
                request.GetExpressions(),
                request.GetOrder(),
                cancellationToken);

        return await zones.ToPaginatedResponseAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomResponse>> GetRoomById(int id, CancellationToken cancellationToken)
    {
        var zone = await _roomRepository
            .FindByAsync<RoomResponse>(
            _ => _.Id == id,
            cancellationToken);

        if (zone is null)
        {
            throw new NotFoundException(nameof(Room), id);
        }

        return zone;
    }

    [HttpPost]
    public async Task<ActionResult<MessageResponse>> CreateRoom(CreateRoomCommand command, CancellationToken cancellationToken)
    {

        var zone = command.Adapt<Room>();
        await _roomRepository.CreateAsync(zone, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new MessageResponse(Resource.CreatedSuccess);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MessageResponse>> UpdateRoom(int id, UpdateRoomCommand command, CancellationToken cancellationToken)
    {
        var zone = await _roomRepository.FindByAsync(
            _ => _.Id == id,
            includeFunc: _ => _
                .Include(x => x.RoomAmenities)
                .Include(x => x.RoomImages)
                .Include(x => x.Seats),
            cancellationToken: cancellationToken);

        if (zone is null)
        {
            throw new NotFoundException(nameof(Room), id);
        }

        command.Adapt(zone);
        await unitOfWork.CommitAsync(cancellationToken);

        return new MessageResponse(Resource.UpdatedSuccess);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<MessageResponse>> DeleteRoom(int id, CancellationToken cancellationToken)
    {
        var zone = await _roomRepository.FindByAsync(
            _ => _.Id == id,
            cancellationToken: cancellationToken);

        if (zone is null)
        {
            throw new NotFoundException(nameof(Room), id);
        }

        await _roomRepository.DeleteAsync(zone);
        await unitOfWork.CommitAsync(cancellationToken);
        return new MessageResponse(Resource.DeletedSuccess);
    }
}
