using Stellaway.Domain.Enums;

namespace Stellaway.DTOs;

public sealed record UpdateEventCommand
{
    public string Name { get; set; } = default!;
    public int Duration { get; set; }
    public bool IsCancelled { get; set; } = false;

    public string? ShortDescription { get; set; } = default!;
    public string? DetailedDescription { get; set; } = default!;
    public string? Director { get; set; } = default!;
    public string? Actors { get; set; } = default!;
    public string? Thumbnail { get; set; } = default!;

    public EventCategory Category { get; set; } = default!;

    //public decimal TotalRevenue { get; set; } = 0.0m; // inject
    public ICollection<UpdateImageRequest> EventImages { get; set; } = new HashSet<UpdateImageRequest>();
}
