using Microsoft.AspNetCore.Mvc;
using Stellaway.Common.Exceptions;
using Stellaway.Domain.Entities.Identities;
using Stellaway.DTOs;
using Stellaway.Repositories;
namespace Stellaway.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController(IUnitOfWork unitOfWork) : ControllerBase
{
    private readonly IGenericRepository<Role> _roleRepository = unitOfWork.Repository<Role>();

    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        return Ok(await _roleRepository.FindAsync<RoleResponse>());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RoleResponse>> GetRoleById(Guid id)
    {
        var role = await _roleRepository.FindByAsync<RoleResponse>(_ => _.Id == id);

        return role != null ? role : throw new NotFoundException(nameof(Role), id);
    }
}