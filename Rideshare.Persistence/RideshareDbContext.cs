using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;
using Rideshare.Domain.Models;

namespace Rideshare.Persistence;

public class RideshareDbContext: IdentityDbContext<User>
{

    public DbSet<TestEntity> TestEntities{ get; set; }
    public DbSet<RideRequest> RideRequests{ get; set; }


    public RideshareDbContext(DbContextOptions<RideshareDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder builder)
{
    builder.UseNpgsql("User ID=postgres;Password=1234;Server=localhost;Port=5432;Database=ridesharedb;Integrated Security=true;Pooling=true;",
        o => o.UseNetTopologySuite());
}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RideshareDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
     

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            entry.Entity.LastModifiedDate = DateTime.Now;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.DateCreated = DateTime.Now;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
