namespace Stellaway.DTOs;

public sealed record CreateImageRequest
{
    public string ImageUrl { get; set; } = default!;
}