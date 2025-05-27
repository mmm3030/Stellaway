namespace Stellaway.DTOs;

public record UserResponse : BaseAuditableEntityResponse<Guid>
{
    public string? UserName { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public string? PhoneNumber { get; set; }
    public bool PhoneNumberConfirmed { get; set; }
    public string? FullName { get; set; }
    public bool IsActive { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<RoleResponse> Roles { get; set; } = new HashSet<RoleResponse>();
}