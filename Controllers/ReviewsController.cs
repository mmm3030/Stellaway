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
public class ReviewsController(
    IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IGenericRepository<Review> _reviewRepository = unitOfWork.Repository<Review>();
    private readonly IGenericRepository<Booking> _bookingRepository = unitOfWork.Repository<Booking>();

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<ReviewResponse>>> GetReviews(
        [FromQuery] GetReviewsQuery request,
        CancellationToken cancellationToken)
    {
        var zones = await _reviewRepository
            .FindAsync<ReviewResponse>(
                request.PageIndex,
                request.PageSize,
                request.GetExpressions(),
                request.GetOrder(),
                cancellationToken);

        return await zones.ToPaginatedResponseAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReviewResponse>> GetReviewById(int id, CancellationToken cancellationToken)
    {
        var zone = await _reviewRepository
            .FindByAsync<ReviewResponse>(
            _ => _.Id == id,
            cancellationToken);

        if (zone is null)
        {
            throw new NotFoundException(nameof(Review), id);
        }

        return zone;
    }

    [HttpPost]
    public async Task<ActionResult<MessageResponse>> CreateReview(CreateReviewCommand command, CancellationToken cancellationToken)
    {
        if (!await _bookingRepository.ExistsByAsync(_ => _.Id == command.BookingId, cancellationToken))
        {
            throw new NotFoundException(nameof(Booking), command.BookingId);
        }

        var zone = command.Adapt<Review>();
        await _reviewRepository.CreateAsync(zone, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return new MessageResponse(Resource.CreatedSuccess);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MessageResponse>> UpdateReview(int id, UpdateReviewCommand command, CancellationToken cancellationToken)
    {
        if (!await _bookingRepository.ExistsByAsync(_ => _.Id == command.BookingId, cancellationToken))
        {
            throw new NotFoundException(nameof(Booking), command.BookingId);
        }

        var zone = await _reviewRepository.FindByAsync(
            _ => _.Id == id,
            cancellationToken: cancellationToken);

        if (zone is null)
        {
            throw new NotFoundException(nameof(Review), id);
        }

        command.Adapt(zone);
        await unitOfWork.CommitAsync(cancellationToken);

        return new MessageResponse(Resource.UpdatedSuccess);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<MessageResponse>> DeleteReview(int id, CancellationToken cancellationToken)
    {
        var zone = await _reviewRepository.FindByAsync(
            _ => _.Id == id,
            cancellationToken: cancellationToken);

        if (zone is null)
        {
            throw new NotFoundException(nameof(Review), id);
        }

        await _reviewRepository.DeleteAsync(zone);
        await unitOfWork.CommitAsync(cancellationToken);
        return new MessageResponse(Resource.DeletedSuccess);
    }
}
