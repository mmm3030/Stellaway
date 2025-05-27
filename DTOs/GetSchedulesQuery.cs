using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Stellaway.Domain.Entities;
using Stellaway.DTOs.Pages;

namespace Stellaway.DTOs;

public sealed record GetSchedulesQuery : PaginationRequest<Schedule>
{
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

    public override Expression<Func<Schedule, bool>> GetExpressions()
    {
        Expression = Expression.And(_ => string.IsNullOrWhiteSpace(EventName) || EF.Functions.Like(_.Event.Name, $"%{EventName}%"));
        Expression = Expression.And(_ => string.IsNullOrWhiteSpace(RoomName) || EF.Functions.Like(_.Room.Name, $"%{RoomName}%"));
        Expression = Expression.And(_ => !StartTimeFrom.HasValue || _.StartTime >= StartTimeFrom);
        Expression = Expression.And(_ => !StartTimeTo.HasValue || _.StartTime <= StartTimeTo.Value.AddDays(1));
        Expression = Expression.And(_ => !EventId.HasValue || _.EventId == EventId);
        Expression = Expression.And(_ => !RoomId.HasValue || _.RoomId == RoomId);

        return Expression;
    }
}
