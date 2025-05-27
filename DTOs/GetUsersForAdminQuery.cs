using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using Stellaway.Domain.Entities.Identities;
using Stellaway.Domain.Enums;
using Stellaway.DTOs.Pages;

namespace Stellaway.DTOs;

public sealed record GetUsersForAdminQuery : PaginationRequest<User>
{
    public string? Search { get; set; }

    public bool? IsActive { get; set; }

    public RoleEnums? Role { get; set; }

    public override Expression<Func<User, bool>> GetExpressions()
    {
        if (!string.IsNullOrWhiteSpace(Search))
        {
            Search = Search.Trim();
            Expression = Expression
                .And(u => EF.Functions.Like(u.PhoneNumber, $"%{Search}%"))
                .Or(u => EF.Functions.Like(u.FullName, $"%{Search}%"))
                .Or(u => EF.Functions.Like(u.UserName, $"%{Search}%"));
        }

        Expression = Expression.And(u => !IsActive.HasValue || u.IsActive == IsActive);
        Expression = Expression.And(u => !Role.HasValue || u.UserRoles.Any(ur => ur.Role.Name == Role.ToString()));

        return Expression;
    }
}