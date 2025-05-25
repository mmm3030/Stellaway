using Stellaway.Domain.Constants;
using Stellaway.Domain.Entities.Identities;

namespace Stellaway.Persistence.SeedData;

internal static class RoleSeed
{
    public static IList<Role> Default => new List<Role>()
    {
        new(RoleName.Admin),
        new(RoleName.User),
        new(RoleName.Staff),
    };
}