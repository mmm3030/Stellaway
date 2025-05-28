using System.Linq.Expressions;
using LinqKit;
using Stellaway.Domain.Entities;
using Stellaway.Domain.Enums;
using Stellaway.DTOs.Pages;

namespace Stellaway.DTOs;

public sealed record GetBookingsQuery : PaginationRequest<Booking>
{
    public double? TotalPriceFrom { get; set; }
    public double? TotalPriceTo { get; set; }
    public BookingStatus? Status { get; set; }
    public BookingMethod? Method { get; set; }
    public Guid? UserId { get; set; }
    public int? ScheduleId { get; set; }
    public int? EventId { get; set; }
    public int? RoomId { get; set; }

    /// <summary>
    /// Format for From is "yyyy-MM-dd" or "MM/dd/yyyy"
    /// </summary>
    /// <example>2021-02-25T00:00:00.000000+00:00</example>
    public DateTimeOffset? CreatedAtFrom { get; set; }

    /// <summary>
    /// Format for To is "yyyy-MM-dd" or "MM/dd/yyyy"
    /// </summary>
    /// <example>2029-03-25T00:00:00.000000+00:00</example>
    public DateTimeOffset? CreatedAtTo { get; set; }

    public override Expression<Func<Booking, bool>> GetExpressions()
    {
        {
            Expression = Expression.And(_ => !TotalPriceFrom.HasValue || _.TotalPrice >= TotalPriceFrom);
            Expression = Expression.And(_ => !TotalPriceTo.HasValue || _.TotalPrice <= TotalPriceTo);
            Expression = Expression.And(_ => !Method.HasValue || _.Method == Method);
            Expression = Expression.And(_ => !Status.HasValue || _.Status == Status);
            Expression = Expression.And(_ => !UserId.HasValue || _.UserId == UserId);
            Expression = Expression.And(_ => !ScheduleId.HasValue || _.ScheduleId == ScheduleId);
            Expression = Expression.And(_ => !EventId.HasValue || _.Schedule.EventId == EventId);
            Expression = Expression.And(_ => !RoomId.HasValue || _.Schedule.RoomId == RoomId);
            Expression = Expression.And(_ => !CreatedAtFrom.HasValue || _.CreatedAt >= CreatedAtFrom);
            Expression = Expression.And(_ => !CreatedAtTo.HasValue || _.CreatedAt <= CreatedAtTo.Value.AddDays(1));

            return Expression;
        }
    }

}