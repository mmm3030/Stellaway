namespace Stellaway.DTOs;

public sealed record CreateReviewCommand
{
    public string Title { get; set; } = default!;
    public string Comment { get; set; } = default!;
    public int Rating { get; set; }

    public Guid BookingId { get; set; }
}
