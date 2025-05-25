using Microsoft.AspNetCore.Identity;

namespace Stellaway.Domain.Entities.Identities;

public class UserRole : IdentityUserRole<Guid>
{
    public virtual Role Role { get; set; } = default!;

    public virtual User User { get; set; } = default!;
}