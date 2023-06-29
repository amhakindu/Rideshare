using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rideshare.Domain.Entities;

namespace Rideshare.Persistence.Configurations.Entities
{
	public class RateEntityConfiguration : IEntityTypeConfiguration<RateEntity>
	{
		public void Configure(EntityTypeBuilder<RateEntity> builder)
		{
			builder.HasData(
				new RateEntity
				{
					Id = 1,
					UserId = "2e4c2829-138d-4e7e-b023-123456789002",
					Rate = 4.5,
					DriverId = 1,
					Description = "Great service!"
				},
				
				new RateEntity
				{
					Id = 2,
					UserId = "2e4c2829-138d-4e7e-b023-123456789002",
					Rate = 3.2,
					DriverId = 2,
					Description = "Average ride experience."
				},
				
				new RateEntity
				{
					Id = 3,
					UserId = "2e4c2829-138d-4e7e-b023-123456789003",
					Rate = 5.0,
					DriverId = 1,
					Description = "Excellent driver, highly recommended."
				},
				
				new RateEntity
				{
					Id = 4,
					UserId = "2e4c2829-138d-4e7e-b023-123456789004",
					Rate = 2.8,
					DriverId = 4,
					Description = "Disappointing service, need improvements."
				},
				
				new RateEntity
				{
					Id = 5,
					UserId = "2e4c2829-138d-4e7e-b023-123456789005",
					Rate = 4.7,
					DriverId = 1,
					Description = "Very friendly driver, enjoyed the ride."
				}
			);
		}
	}
}
