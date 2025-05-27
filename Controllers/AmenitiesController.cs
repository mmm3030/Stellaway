using Mapster;
using Microsoft.AspNetCore.Mvc;
using Stellaway.Common.Exceptions;
using Stellaway.Common.Resources;
using Stellaway.Domain.Entities;
using Stellaway.DTOs;
using Stellaway.Repositories;

namespace Stellaway.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AmenitiesController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IGenericRepository<Amenity> _amenityRepository = unitOfWork.Repository<Amenity>();

    [HttpGet]
    public async Task<IActionResult> GetAmenities(CancellationToken cancellationToken)
    {
        return Ok(await _amenityRepository.FindAsync<AmenityResponse>(cancellationToken: cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AmenityResponse>> GetAmenityById(int id, CancellationToken cancellationToken)
    {

        var device = await _amenityRepository
            .FindByAsync<AmenityResponse>(
            x => x.Id == id,
            cancellationToken);

        if (device == null)
        {
            throw new NotFoundException(nameof(Amenity), id);
        }

        return device;
    }

    [HttpPost]
    public async Task<ActionResult<MessageResponse>> CreateAmenity(CreateAmenityCommand command, CancellationToken cancellationToken)
    {

        var device = command.Adapt<Amenity>();

        await _amenityRepository.CreateAsync(device, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new MessageResponse(Resource.CreatedSuccess);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MessageResponse>> UpdateAmenity(int id, UpdateAmenityCommand command, CancellationToken cancellationToken)
    {

        var device = await _amenityRepository
            .FindByAsync(
            x => x.Id == id,
            cancellationToken: cancellationToken);

        if (device == null)
        {
            throw new NotFoundException(nameof(Amenity), id);
        }

        command.Adapt(device);

        await unitOfWork.CommitAsync(cancellationToken);

        return new MessageResponse(Resource.DeviceUpdatedSuccess);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<MessageResponse>> DeleteAmenity(int id, CancellationToken cancellationToken)
    {

        var device = await _amenityRepository
            .FindByAsync(x =>
            x.Id == id, cancellationToken: cancellationToken);

        if (device == null)
        {
            throw new NotFoundException(nameof(Amenity), id);
        }

        await _amenityRepository.DeleteAsync(device);

        await unitOfWork.CommitAsync(cancellationToken);

        return new MessageResponse(Resource.DeletedSuccess);
    }

}
