using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stellaway.Domain.Entities;
using Stellaway.Domain.Entities.Identities;

namespace Stellaway.Persistence.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(options)
{
    private const string Prefix = "AspNet";

    public DbSet<Room> Rooms { get; set; } = default!;
    public DbSet<RoomAmenity> RoomAmenities { get; set; } = default!;
    public DbSet<Amenity> Amenities { get; set; } = default!;
    public DbSet<RoomImage> RoomImages { get; set; } = default!;
    public DbSet<Seat> Seats { get; set; } = default!;
    public DbSet<Schedule> Schedules { get; set; } = default!;
    public DbSet<Event> Events { get; set; } = default!;
    public DbSet<EventImage> EventImages { get; set; } = default!;
    public DbSet<Ticket> Tickets { get; set; } = default!;
    public DbSet<Booking> Bookings { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tableName = entityType.GetTableName();
            if (tableName != null && tableName.StartsWith(Prefix))
            {
                entityType.SetTableName(tableName.Substring(6));
            }
        }

        modelBuilder.Entity<UserRole>(b =>
        {
            b.HasOne(e => e.Role)
                .WithMany(e => e.UserRoles)
                .HasForeignKey(ur => ur.RoleId);

            b.HasOne(e => e.User)
                .WithMany(e => e.UserRoles)
                .HasForeignKey(ur => ur.UserId);
        });

        modelBuilder.Entity<RoomAmenity>(b =>
        {
            b.HasKey(c => new { c.RoomId, c.AmenityId });
        });
    }
}