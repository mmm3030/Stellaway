namespace Stellaway.DTOs;

public sealed record CreateAmenityCommand
{
    public string Name { get; set; } = default!;
}
