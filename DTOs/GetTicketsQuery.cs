using System.Linq.Expressions;
using LinqKit;
using Stellaway.Domain.Entities;
using Stellaway.Domain.Enums;
using Stellaway.DTOs.Pages;

namespace Stellaway.DTOs;

public sealed record GetTicketsQuery : PaginationRequest<Ticket>
{

    public double? PriceFrom { get; set; }
    public double? PriceTo { get; set; }
    public bool? IsUsed { get; set; }
    public Guid? UserId { get; set; }

    public string? EventName { get; set; }
    public string? RoomName { get; set; }

    /// <summary>
    /// Format for From is "yyyy-MM-dd" or "MM/dd/yyyy"
    /// </summary>
    /// <example>2021-02-25T00:00:00.000000+00:00</example>
    public DateTimeOffset? StartTimeFrom { get; set; }

    /// <summary>
    /// Format for To is "yyyy-MM-dd" or "MM/dd/yyyy"
    /// </summary>
    /// <example>2029-03-25T00:00:00.000000+00:00</example>
    public DateTimeOffset? StartTimeTo { get; set; }
    public int? EventId { get; set; }
    public int? RoomId { get; set; }

    public override Expression<Func<Ticket, bool>> GetExpressions()
    {

        Expression = Expression.And(_ => !PriceFrom.HasValue || _.Price >= PriceFrom);
        Expression = Expression.And(_ => !PriceTo.HasValue || _.Price <= PriceTo);
        Expression = Expression.And(_ => !IsUsed.HasValue || _.IsUsed == IsUsed);
        Expression = Expression.And(_ => !UserId.HasValue || _.Booking.UserId == UserId);
        Expression = Expression.And(_ => string.IsNullOrWhiteSpace(EventName) || _.Booking.Schedule.Event.Name.Contains(EventName));
        Expression = Expression.And(_ => string.IsNullOrWhiteSpace(RoomName) || _.Booking.Schedule.Room.Name.Contains(RoomName));
        Expression = Expression.And(_ => !StartTimeFrom.HasValue || _.Booking.Schedule.StartTime >= StartTimeFrom);
        Expression = Expression.And(_ => !StartTimeTo.HasValue || _.Booking.Schedule.StartTime <= StartTimeTo.Value.AddDays(1));
        Expression = Expression.And(_ => !EventId.HasValue || _.Booking.Schedule.EventId == EventId);
        Expression = Expression.And(_ => !RoomId.HasValue || _.Booking.Schedule.RoomId == RoomId);

        Expression = Expression.And(_ => _.Booking.Status == BookingStatus.Completed);

        return Expression;
    }
}
