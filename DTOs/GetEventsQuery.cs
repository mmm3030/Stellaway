using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Stellaway.Domain.Entities;
using Stellaway.Domain.Enums;
using Stellaway.DTOs.Pages;

namespace Stellaway.DTOs;

public sealed record GetEventsQuery : PaginationRequest<Event>
{
    public string? Search { get; set; }
    public EventCategory? Category { get; set; }

    public string? Director { get; set; } = default!;
    public string? Actors { get; set; } = default!;

    public bool? IsCancelled { get; set; }

    public override Expression<Func<Event, bool>> GetExpressions()
    {
        if (!string.IsNullOrWhiteSpace(Search))
        {
            Search = Search.Trim();
            Expression = Expression
                .Or(sta => EF.Functions.Like(sta.Name, $"%{Search}%"))
                .Or(sta => EF.Functions.Like(sta.ShortDescription, $"%{Search}%"))
                .Or(sta => EF.Functions.Like(sta.DetailedDescription, $"%{Search}%"));
        }
        Expression = Expression.And(_ => !Category.HasValue || _.Category == Category);
        Expression = Expression.And(_ => string.IsNullOrWhiteSpace(Director) || EF.Functions.Like(_.Director, $"%{Director}%"));
        Expression = Expression.And(_ => string.IsNullOrWhiteSpace(Actors) || EF.Functions.Like(_.Actors, $"%{Actors}%"));
        Expression = Expression.And(_ => !IsCancelled.HasValue || _.IsCancelled == IsCancelled);

        return Expression;
    }
}
