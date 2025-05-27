using System.ComponentModel.DataAnnotations.Schema;
using Stellaway.Domain.Common;
using Stellaway.Domain.Enums;

namespace Stellaway.Domain.Entities;

public class Event : BaseAuditableEntity<int>
{
    public string Name { get; set; } = default!;
    public int Duration { get; set; }
    public bool IsCancelled { get; set; } = false;

    public string? ShortDescription { get; set; } = default!;
    public string? DetailedDescription { get; set; } = default!;
    public string? Director { get; set; } = default!;
    public string? Actors { get; set; } = default!;
    public string? Thumbnail { get; set; } = default!;

    [Column(TypeName = "nvarchar(24)")]
    public EventCategory Category { get; set; }

    //public decimal TotalRevenue { get; set; } = 0.0m; // inject
    public virtual ICollection<EventImage> EventImages { get; set; } = new HashSet<EventImage>();

    public virtual ICollection<Schedule> Schedules { get; set; } = new HashSet<Schedule>();

}
