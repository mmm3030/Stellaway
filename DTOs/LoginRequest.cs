using System.ComponentModel;

namespace Stellaway.DTOs;

public sealed record LoginRequest
{
    [DefaultValue("admin")]
    public string Username { get; init; } = default!;

    [DefaultValue("admin")]
    public string Password { get; init; } = default!;

}