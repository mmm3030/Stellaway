using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stellaway.Common.Exceptions;
using Stellaway.Common.Resources;
using Stellaway.Domain.Entities.Identities;
using Stellaway.DTOs;
using Stellaway.DTOs.Pages;
using Stellaway.Repositories;

namespace Stellaway.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController(
    UserManager<User> userManager,
    IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IGenericRepository<User> _userRepository = unitOfWork.Repository<User>();

    [HttpPost("change-password/{userId}")]
    public async Task<ActionResult<MessageResponse>> ChangePassword(Guid userId, ChangePasswordRequest request)
    {
        var user = await _userRepository.FindByAsync(_ => _.Id == userId);

        if (user is null)
        {
            throw new NotFoundException(nameof(User), userId);
        }

        if (!await userManager.HasPasswordAsync(user))
        {
            throw new BadRequestException(Resource.UserNotHavePassword);
        }

        var result = await userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);

        if (!result.Succeeded)
        {
            throw new ValidationBadRequestException(result.Errors);
        }

        return new MessageResponse(Resource.PasswordChangeSuccess);
    }

    [HttpGet]
    public async Task<ActionResult<PaginatedResponse<UserResponse>>> GetUsers(
         [FromQuery] GetUsersForAdminQuery request,
         CancellationToken cancellationToken)
    {
        var users = await _userRepository
            .FindAsync<UserResponse>(
                request.PageIndex,
                request.PageSize,
                request.GetExpressions(),
                request.GetOrder(),
                cancellationToken);

        return await users.ToPaginatedResponseAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponse>> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .FindByAsync<UserResponse>(x => x.Id == id, cancellationToken);

        if (user == null)
        {
            throw new NotFoundException(nameof(User), id);
        }

        return user;
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MessageResponse>> UpdateUser(Guid id, UpdateUserForAdminCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByAsync(x => x.Id == id, cancellationToken: cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(nameof(User), id);
        }
        request.Adapt(user);
        await userManager.UpdateNormalizedEmailAsync(user);
        await unitOfWork.CommitAsync(cancellationToken);
        return new MessageResponse(Resource.UserUpdatedProfileSuccess);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<MessageResponse>> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByAsync(x => x.Id == id, cancellationToken: cancellationToken);

        if (user is null)
        {
            throw new NotFoundException(nameof(User), id);
        }

        await _userRepository.DeleteAsync(user);
        await unitOfWork.CommitAsync(cancellationToken);

        return new MessageResponse(Resource.DeletedSuccess);
    }

}
