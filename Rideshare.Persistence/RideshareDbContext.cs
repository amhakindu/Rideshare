using System.Reflection;
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
	public DbSet<RateEntity> RateEntities{ get; set; }
	public DbSet<Connection> Connections { get; set; }

	public RideshareDbContext() {}
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

		modelBuilder.HasDbFunction(typeof(RideshareDbContext)
			.GetMethod(nameof(RideshareDbContext.haversine_distance), BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(double), typeof(double), typeof(double), typeof(double)}, null))
			.HasSchema("public")
    		.HasName("haversine_distance");
		
		modelBuilder.Entity<Driver>()
			.HasOne(d => d.User)
			.WithOne()
			.HasForeignKey<Driver>(d => d.UserId)
			.OnDelete(DeleteBehavior.Cascade);
		
		modelBuilder.Entity<Driver>()
			.Property<DateTime>("DateCreated")
			.HasColumnType("timestamp with time zone"); 
		
		modelBuilder.Entity<Driver>()
			.Property<DateTime>("LastModifiedDate")
			.HasColumnType("timestamp with time zone"); 
	
		modelBuilder.Entity<Vehicle>()
			.Property<DateTime>("DateCreated")
			.HasColumnType("timestamp with time zone"); 
		
		modelBuilder.Entity<Vehicle>()
			.Property<DateTime>("LastModifiedDate")
			.HasColumnType("timestamp with time zone"); 

		modelBuilder.Entity<RateEntity>()
			.Property<DateTime>("DateCreated")
			.HasColumnType("timestamp with time zone"); 
		
		modelBuilder.Entity<RateEntity>()
			.Property<DateTime>("LastModifiedDate")
			.HasColumnType("timestamp with time zone"); 

		 modelBuilder.Entity<ApplicationUser>()
			.HasIndex(u => u.PhoneNumber)
			.IsUnique();

		 modelBuilder.Entity<ApplicationUser>()
			.Property<DateTime>("CreatedAt")
			.HasColumnType("timestamp with time zone");

		 modelBuilder.Entity<ApplicationUser>()
			.Property<DateTime?>("LastLogin")
			.HasColumnType("timestamp with time zone");
		
		modelBuilder.Entity<ApplicationUser>()
			.Property<DateTime?>("RefreshTokenExpiryTime")
			.HasColumnType("timestamp with time zone");

		modelBuilder.Entity<GeographicalLocation>()
			.Property(g => g.Coordinate)
			.HasColumnType("geometry(Point,4326)");

		modelBuilder.Entity<GeographicalLocation>()
			.Property<DateTime>("DateCreated")
			.HasColumnType("timestamp with time zone"); 
		
		modelBuilder.Entity<GeographicalLocation>()
			.Property<DateTime>("LastModifiedDate")
			.HasColumnType("timestamp with time zone"); 
			
		modelBuilder.Entity<RideRequest>()
			.HasOne(riderequest => riderequest.MatchedRide)
			.WithMany(rideoffer => rideoffer.Matches)
			.HasForeignKey(riderequest => riderequest.MatchedRideId);

		modelBuilder.Entity<RideRequest>()
			.HasOne<GeographicalLocation>(riderequest => riderequest.Origin)
			.WithMany()
			.HasForeignKey(riderequest => riderequest.OriginId);

		modelBuilder.Entity<RideRequest>()
			.HasOne<GeographicalLocation>(riderequest => riderequest.Destination)
			.WithMany()
			.HasForeignKey(riderequest => riderequest.DestinationId);

		modelBuilder.Entity<RideRequest>()
			.Property<DateTime>("DateCreated")
			.HasColumnType("timestamp with time zone"); 
		
		modelBuilder.Entity<RideRequest>()
			.Property<DateTime>("LastModifiedDate")
			.HasColumnType("timestamp with time zone"); 

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

		modelBuilder.Entity<RideOffer>()
			.Property<DateTime>("DateCreated")
			.HasColumnType("timestamp with time zone"); 
		
		modelBuilder.Entity<RideOffer>()
			.Property<DateTime>("LastModifiedDate")
			.HasColumnType("timestamp with time zone"); 
	}


    [DbFunction("public", "haversine_distance")]
    public static double haversine_distance(double originX, double originY, double destX, double destY)
    {
        return 0.0;
    }
	public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{

		foreach (var entry in ChangeTracker.Entries<BaseEntity>())
		{
			entry.Entity.LastModifiedDate = DateTime.UtcNow;

			if (entry.State == EntityState.Added)
			{
				entry.Entity.DateCreated = DateTime.UtcNow;
			}
		}

		return base.SaveChangesAsync(cancellationToken);
	}
}
