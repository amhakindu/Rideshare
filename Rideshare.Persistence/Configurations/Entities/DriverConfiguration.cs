using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rideshare.Domain.Entities;

namespace Rideshare.Persistence.Configurations.Entities
{
	public class DriverConfiguration : IEntityTypeConfiguration<Driver>
	{
		public void Configure(EntityTypeBuilder<Driver> builder)
		{
			builder.HasData(
				new Driver
				{
					Id = 1,
					UserId = "2e4c2829-138d-4e7e-b023-123456789105",
					Experience = 3.5,
					Verified = true,
					Address = "Lafto",
					LicenseNumber = "12345678",
					License =  "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688116035/sax8xmpkspofh7s9iai5.png"
				},
				new Driver
				{
					Id = 2,
					UserId = "2e4c2829-138d-4e7e-b023-123456789104",
					Experience = 2.0,
					Verified = false,
					Address = "Arat kilo",
					LicenseNumber = "87654321",
					License =  "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688116035/sax8xmpkspofh7s9iai5.png"
				},
				new Driver
				{
					Id = 3,
					UserId = "2e4c2829-138d-4e7e-b023-123456789103",
					Experience = 4.8,
					Verified = true,
					Address = "Debrezeyit",
					LicenseNumber = "98765432",
					License = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688116035/sax8xmpkspofh7s9iai5.png"
				},
				new Driver
				{
					Id = 4,
					UserId = "2e4c2829-138d-4e7e-b023-123456789102",
					Experience = 1.2,
					Verified = false,
					Address = "Nazrit",
					LicenseNumber = "23456789",
					License = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688116035/sax8xmpkspofh7s9iai5.png"
				},
				new Driver
				{
					Id = 5,
					UserId = "2e4c2829-138d-4e7e-b023-123456789101",
					Experience = 5.0,
					Verified = true,
					Address = "Weraileu",
					LicenseNumber = "98765432",
					License = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688116035/sax8xmpkspofh7s9iai5.png"
				},
				
				
				new Driver
				{
					Id = 8,
					UserId = "012d3aef-4b56-471c-86d9-123456789004",
					Experience = 2,
					Verified = true,
					Address = "Merkato, Addis Ababa",
					LicenseNumber = "GHI123456",
					License = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688116035/sax8xmpkspofh7s9iai5.png"
				},
				new Driver
				{
					Id = 9,
					UserId = "12b3c4d5-67e8-490f-1a2b-123456789008",
					Experience = 3,
					Verified = false,
					Address = "Kirkos Subcity, Woreda 04, Addis Ababa",
					LicenseNumber = "JKL123456",
					License = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688116035/sax8xmpkspofh7s9iai5.png"
				},
				new Driver
				{
					Id = 10,
					UserId = "1234e56f-7890-4c1d-23e4-123456789012",
					Experience = 4,
					Verified = true,
					Address = "Arat Kilo, Addis Ababa",
					LicenseNumber = "MNO123456",
					License = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688116035/sax8xmpkspofh7s9iai5.png"
				},
				new Driver
				{
					Id = 11,
					UserId = "3e4f56a7-8901-4b2c-3d4e-123456789016",
					Experience = 5,
					Verified = false,
					Address = "Bole Subcity, Woreda 07, Addis Ababa",
					LicenseNumber = "PQR123456",
					License = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688116035/sax8xmpkspofh7s9iai5.png"
				},
				new Driver
				{
					Id = 12,
					UserId = "d5eade8b-11a4-4e26-b7e6-123456789020",
					Experience = 1,
					Verified = true,
					Address = "Kazanchis, Addis Ababa",
					LicenseNumber = "STU123456",
					License = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688116035/sax8xmpkspofh7s9iai5.png"
				},
				new Driver
				{
					Id = 13,
					UserId = "e7f89012-3a4b-567c-e7f8-123456789024",
					Experience = 2,
					Verified = false,
					Address = "Bole Subcity, Woreda 03, Addis Ababa",
					LicenseNumber = "VWX123456",
					License = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688116035/sax8xmpkspofh7s9iai5.png"
				},
				new Driver
				{
					Id = 14,
					UserId = "6789a0b1-c23d-4e56-6789-123456789028",
					Experience = 3,
					Verified = true,
					Address = "Bole Subcity, Woreda 03, Addis Ababa",
					LicenseNumber = "YZA123456",
					License = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688116035/sax8xmpkspofh7s9iai5.png"
				},
				new Driver
				{
					Id = 15,
					UserId = "e7f89012-3a4b-567c-e7f8-123456789032",
					Experience = 4,
					Verified = false,
					Address = "Bole Subcity, Woreda 03, Addis Ababa",
					LicenseNumber = "BCD123456",
					License = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688116035/sax8xmpkspofh7s9iai5.png"
				},
				new Driver
				{
					Id = 16,
					UserId = "6789a0b1-c23d-4e56-6789-123456789036",
					Experience = 5,
					Verified = true,
					Address = "Bole Subcity, Woreda 03, Addis Ababa",
					LicenseNumber = "EFG123456",
					License = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688116035/sax8xmpkspofh7s9iai5.png"
				},
				new Driver
				{
					Id = 17,
					UserId = "01a2b34c-5d6e-4890-1a2b-123456789040",
					Experience = 1,
					Verified = false,
					Address = "Megenagna, Addis Ababa",
					LicenseNumber = "HIJ123456",
					License = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688116035/sax8xmpkspofh7s9iai5.png"
				}
			);
		}
	}
}
