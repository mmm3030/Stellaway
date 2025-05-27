namespace Stellaway.DTOs;

public sealed record RoleResponse
{
    public Guid Id { get; set; }

    public string? Name { get; set; }
}