namespace Stellaway.DTOs;

public sealed record UpdateUserForAdminCommand
{

    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? AvatarUrl { get; set; }

    public string? PhoneNumber { get; set; }

    public bool IsActive { get; set; }
}