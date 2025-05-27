namespace Stellaway.DTOs;

public sealed record UpdateProfileCommand
{
    public string? Email { get; set; }
    public string? FullName { get; set; }
    public string? AvatarUrl { get; set; }
}