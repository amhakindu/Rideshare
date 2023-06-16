using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;
using Rideshare.Domain.Models;

namespace Rideshare.Persistence;

public class RideshareDbContext: IdentityDbContext<User>
{

    public DbSet<TestEntity> TestEntities{ get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<RideRequest> RideRequests{ get; set; }
    public DbSet<Feedback> FeedBackEntities { get; set; }
    public RideshareDbContext(DbContextOptions<RideshareDbContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }
 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("postgis");
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RideshareDbContext).Assembly);
    }

	public DbSet<RateEntity> RateEntities{ get; set; }


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
