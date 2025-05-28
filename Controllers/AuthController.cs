using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stellaway.Common.Exceptions;
using Stellaway.Common.Resources;
using Stellaway.Domain.Constants;
using Stellaway.Domain.Entities.Identities;
using Stellaway.DTOs;

namespace Stellaway.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController(UserManager<User> userManager) : ControllerBase
{

    [HttpPost("login")]
    public async Task<ActionResult<UserResponse>> Login(LoginRequest request)
    {
        var user = await userManager.FindByNameAsync(request.Username);

        if (user == null)
        {
            throw new UnauthorizedAccessException(Resource.Unauthorized);
        }

        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            throw new UnauthorizedAccessException(Resource.Unauthorized);
        }

        if (!user.IsActive)
        {
            throw new UnauthorizedAccessException(Resource.Unauthorized);
        }

        return user.Adapt<UserResponse>();
    }

    [HttpPost("register")]
    public async Task<ActionResult<MessageResponse>> Register(RegisterRequest request)
    {
        var user = await userManager.FindByNameAsync(request.Username);

        if (user is not null)
        {
            throw new BadRequestException("Username đã tồn tại");
        }

        user = new User
        {
            UserName = request.Username,
            IsActive = true,
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new ValidationBadRequestException(result.Errors);
        }

        result = await userManager.AddToRoleAsync(user, RoleName.User);

        if (!result.Succeeded)
        {
            throw new ValidationBadRequestException(result.Errors);
        }

        return new MessageResponse(Resource.CreatedSuccess);

    }

}
