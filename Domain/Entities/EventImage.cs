using Stellaway.Domain.Common;

namespace Stellaway.Domain.Entities;

public class EventImage : BaseEntity<int>
{
    public string ImageUrl { get; set; } = default!;

    public int EventId { get; set; }
    public virtual Event Event { get; set; } = default!;
}
