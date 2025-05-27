using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Stellaway.Domain.Entities;
using Stellaway.Domain.Enums;
using Stellaway.DTOs.Pages;

namespace Stellaway.DTOs;

public sealed record GetRoomsQuery : PaginationRequest<Room>
{

    public string? Search { get; set; }
    public string? Location { get; set; }
    public int? Capacity { get; set; }
    public int? Length { get; set; }
    public int? Width { get; set; }
    public int? Height { get; set; }
    public RoomStatus? Status { get; set; }
    public RoomCategory? Category { get; set; }

    public override Expression<Func<Room, bool>> GetExpressions()
    {
        if (!string.IsNullOrWhiteSpace(Search))
        {
            Search = Search.Trim();
            Expression = Expression
                .Or(sta => EF.Functions.Like(sta.Name, $"%{Search}%"))
                .Or(sta => EF.Functions.Like(sta.Description, $"%{Search}%"));
        }

        Expression = Expression.And(_ => string.IsNullOrWhiteSpace(Location) || EF.Functions.Like(_.Location, $"%{Location}%"));
        Expression = Expression.And(_ => !Capacity.HasValue || _.Capacity == Capacity);
        Expression = Expression.And(_ => !Length.HasValue || _.Length == Length);
        Expression = Expression.And(_ => !Width.HasValue || _.Width == Width);
        Expression = Expression.And(_ => !Height.HasValue || _.Height == Height);
        Expression = Expression.And(_ => !Status.HasValue || _.Status == Status);
        Expression = Expression.And(_ => !Category.HasValue || _.Category == Category);

        return Expression;
    }
}
