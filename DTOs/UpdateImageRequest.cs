namespace Stellaway.DTOs;

public sealed record UpdateImageRequest
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = default!;
}