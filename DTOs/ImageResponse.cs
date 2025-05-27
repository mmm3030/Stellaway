namespace Stellaway.DTOs;

public sealed record ImageResponse
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = default!;
}
