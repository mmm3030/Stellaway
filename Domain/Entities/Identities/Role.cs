using Microsoft.AspNetCore.Identity;

namespace Stellaway.Domain.Entities.Identities;

public class Role : IdentityRole<Guid>
{
    public Role()
    {
    }

    public Role(string roleName) : this()
    {
        Name = roleName;
    }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
}