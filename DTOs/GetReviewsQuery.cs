using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Stellaway.Domain.Entities;
using Stellaway.DTOs.Pages;

namespace Stellaway.DTOs;

public sealed record GetReviewsQuery : PaginationRequest<Review>
{
    public string? Title { get; set; }
    public string? Comment { get; set; }
    public int? Rating { get; set; }

    public Guid? BookingId { get; set; }
    public Guid? UserId { get; set; }
    public int? ScheduleId { get; set; }
    public int? EventId { get; set; }
    public int? RoomId { get; set; }

    public override Expression<Func<Review, bool>> GetExpressions()
    {
        Expression = Expression.And(sta => string.IsNullOrWhiteSpace(Comment) || EF.Functions.Like(sta.Comment, $"%{Comment}%"));
        Expression = Expression.And(sta => string.IsNullOrWhiteSpace(Title) || EF.Functions.Like(sta.Title, $"%{Title}%"));
        Expression = Expression.And(_ => !Rating.HasValue || _.Rating == Rating);
        Expression = Expression.And(_ => !BookingId.HasValue || _.BookingId == BookingId);
        Expression = Expression.And(_ => !UserId.HasValue || _.Booking.UserId == UserId);
        Expression = Expression.And(_ => !ScheduleId.HasValue || _.Booking.ScheduleId == ScheduleId);
        Expression = Expression.And(_ => !EventId.HasValue || _.Booking.Schedule.EventId == EventId);
        Expression = Expression.And(_ => !RoomId.HasValue || _.Booking.Schedule.RoomId == RoomId);

        return Expression;
    }
}
