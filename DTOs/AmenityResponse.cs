namespace Stellaway.DTOs;

public sealed record AmenityResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
}
