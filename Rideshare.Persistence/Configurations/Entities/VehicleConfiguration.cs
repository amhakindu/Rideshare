using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rideshare.Domain.Entities;

namespace Rideshare.Persistence.Configurations.Entities
{
	public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
	{		
		DateTime GetRandomDate(DateTime start, DateTime end, Random random)
		{
			var range = (end - start).Days;
			return start.AddDays(random.Next(range));
		}
		
		public void Configure(EntityTypeBuilder<Vehicle> builder)
		{
			var random = new Random();
			var startDate = new DateTime(2023, 1, 1).ToUniversalTime();
			var endDate = DateTime.UtcNow;
			builder.HasData(
				new Vehicle
				{
					Id = 1,
					PlateNumber = "ABC123",
					NumberOfSeats = 40,
					Model = "Toyota Camry",
					Libre = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688100796/xsabqbbff2jc7kqc6efx.pdf",
					DriverId = 1,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new Vehicle
				{
					Id = 2,
					PlateNumber = "XYZ789",
					NumberOfSeats = 5,
					Model = "Honda Civic",
					Libre =  "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688100780/m39jk106ug96qt7e13me.pdf",
					DriverId = 2,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new Vehicle
				{
					Id = 3,
					PlateNumber = "DEF456",
					NumberOfSeats = 7,
					Model = "Ford Explorer",
					Libre = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688100768/jo9dzde295c0ebkjr7hz.pdf",
					DriverId = 3,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new Vehicle
				{
					Id = 4,
					PlateNumber = "MNO789",
					NumberOfSeats = 4,
					Model = "Hyundai Elantra",
					Libre =  "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688100754/xp7cy2ltzynyrkwlufzp.pdf",
					DriverId = 4,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new Vehicle
				{
					Id = 5,
					PlateNumber = "PQR012",
					NumberOfSeats = 6,
					Model = "Nissan Altima",
					Libre =  "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688100699/iofw5wmwav58y9pems5w.pdf",
					DriverId = 5,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				
				// Add 10 more vehicles
				
				
				new Vehicle
				{
					Id = 8,
					PlateNumber = "STU901",
					NumberOfSeats = 5,
					Model = "BMW 3 Series",
					Libre = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688100796/xsabqbbff2jc7kqc6efx.pdf",
					DriverId = 8,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new Vehicle
				{
					Id = 9,
					PlateNumber = "VWX234",
					NumberOfSeats = 8,
					Model = "Audi A4",
					Libre = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688100796/xsabqbbff2jc7kqc6efx.pdf",
					DriverId = 9,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new Vehicle
				{
					Id = 10,
					PlateNumber = "YZA567",
					NumberOfSeats = 3,
					Model = "Mercedes-Benz C-Class",
					Libre = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688100796/xsabqbbff2jc7kqc6efx.pdf",
					DriverId = 10,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				}
			);
		}
	}
}
