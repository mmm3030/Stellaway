namespace Stellaway.DTOs;

public sealed record RegisterRequest
{
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
}
