using Rideshare.Domain.Common;
using Rideshare.Domain.Models;
using Rideshare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Rideshare.Persistence;

public class RideshareDbContext: IdentityDbContext<ApplicationUser,ApplicationRole,string>
{
	public DbSet<RideOffer> RideOffers { get; set; }
	public DbSet<GeographicalLocation> Locations { get; set; }    
	public DbSet<Vehicle> Vehicles { get; set; }
	public DbSet<Driver> Drivers { get; set; }
	public DbSet<RideRequest> RideRequests{ get; set; }
	public DbSet<Feedback> FeedBackEntities { get; set; }
	public DbSet<Connection> Connections { get; set; }

	public RideshareDbContext(DbContextOptions<RideshareDbContext> options)
		: base(options)
	{
		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
		AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.HasPostgresExtension("postgis");
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(RideshareDbContext).Assembly);
		
		
		modelBuilder.Entity<Driver>()
			.HasOne(d => d.User)
			.WithOne()
			.HasForeignKey<Driver>(d => d.UserId)
			.OnDelete(DeleteBehavior.Cascade);
		 modelBuilder.Entity<ApplicationUser>()
			.HasIndex(u => u.PhoneNumber)
			.IsUnique();

		modelBuilder.Entity<GeographicalLocation>()
			.Property(g => g.Coordinate)
			.HasColumnType("geometry(Point,4326)");
			
		modelBuilder.Entity<RideRequest>()
			.HasOne(riderequest => riderequest.MatchedRide)
			.WithMany(rideoffer => rideoffer.Matches)
			.HasForeignKey(riderequest => riderequest.MatchedRideId);
			
		modelBuilder.Entity<RideOffer>()
			.HasMany(rideoffer => rideoffer.Matches)
			.WithOne(riderequest => riderequest.MatchedRide);

		modelBuilder.Entity<RideOffer>()
			.HasOne<Driver>(rideoffer => rideoffer.Driver)
			.WithMany()
			.HasForeignKey(rideoffer => rideoffer.DriverId);
		
		modelBuilder.Entity<RideOffer>()
			.HasOne<Vehicle>(rideoffer => rideoffer.Vehicle)
			.WithMany()
			.HasForeignKey(rideoffer => rideoffer.VehicleId);

		modelBuilder.Entity<RideOffer>()
			.HasOne<GeographicalLocation>(rideoffer => rideoffer.CurrentLocation)
			.WithMany()
			.HasForeignKey(rideoffer => rideoffer.CurrentLocationId);

		modelBuilder.Entity<RideOffer>()
			.HasOne<GeographicalLocation>(rideoffer => rideoffer.Destination)
			.WithMany()
			.HasForeignKey(rideoffer => rideoffer.DestinationId);

		modelBuilder.Entity<RideRequest>()
			.HasOne<GeographicalLocation>(riderequest => riderequest.Origin)
			.WithMany()
			.HasForeignKey(riderequest => riderequest.OriginId);

		modelBuilder.Entity<RideRequest>()
			.HasOne<GeographicalLocation>(riderequest => riderequest.Destination)
			.WithMany()
			.HasForeignKey(riderequest => riderequest.DestinationId);
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
