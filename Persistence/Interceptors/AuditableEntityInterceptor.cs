using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Stellaway.Domain.Common.Interfaces;

namespace Stellaway.Persistence.Interceptors;

public class AuditableEntityInterceptor(IHttpContextAccessor httpContextAccessor) : SaveChangesInterceptor
{
    private const string Anonymous = nameof(Anonymous);
    private readonly string CurrentUserId = httpContextAccessor.HttpContext?.User?.Identity?.Name ?? Anonymous;

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = CurrentUserId;
                entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.ModifiedBy = CurrentUserId;
                entry.Entity.ModifiedAt = DateTimeOffset.UtcNow;
            }

            // nếu dùng delete này thì tất cả entity phải kế thừa BaseAuditableEntity
            //if (entry.State == EntityState.Deleted)
            //{
            //    entry.State = EntityState.Modified;
            //    entry.Entity.DeletedBy = CurrentUserId;
            //    entry.Entity.DeletedAt = DateTimeOffset.UtcNow;
            //}
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry != null &&
            r.TargetEntry.Metadata.IsOwned() &&
            (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}