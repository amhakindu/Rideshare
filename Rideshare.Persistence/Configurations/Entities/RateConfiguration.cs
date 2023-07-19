using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rideshare.Domain.Entities;

namespace Rideshare.Persistence.Configurations.Entities
{
	public class RateEntityConfiguration : IEntityTypeConfiguration<RateEntity>
	{
		public int GetRandomInt(int start, int end)
		{
			Random random = new Random();
			return random.Next(start, end + 1);
		}
		DateTime GetRandomDate(DateTime start, DateTime end, Random random)
		{
			var range = (end - start).Days;
			return start.AddDays(random.Next(range));
		}
		public void Configure(EntityTypeBuilder<RateEntity> builder)
		{
			var random = new Random();
			var startDate = new DateTime(2023, 1, 1);
			var userIds = new List<string>(){
				"d5eade8b-11a4-4e26-b7e6-123456789000",
				"123a8b56-c7d9-4e0a-89bf-123456789001",
				"456b8d23-e9f0-4c1d-ace6-123456789002",
				"789c0e12-3d45-4a6f-8b27-123456789003",
				"012d3aef-4b56-471c-86d9-123456789004",
				"345e6bcd-7890-4def-12ab-123456789005",
				"678f9de0-1c23-4ab4-5678-123456789006",
				"90a1b234-c5d6-4e7f-8901-123456789007",
				"12b3c4d5-67e8-490f-1a2b-123456789008",
				"34e5f6a7-8901-4bcd-23ef-123456789009",
				"5678a9b0-c1d2-4e3f-4a5b-123456789010",
				"8901c2d3-4e56-47f8-9012-123456789011",
				"1234e56f-7890-4c1d-23e4-123456789012",
				"4567f89a-0b12-43c4-5678-123456789013",
				"7890a1b2-34c5-4d6e-7890-123456789014",
				"0c1d23e4-56f7-4980-12ab-123456789015",
				"3e4f56a7-8901-4b2c-3d4e-123456789016",
				"6789a0b1-c23d-4e5f-6789-123456789017",
				"90a1b23c-45d6-4e7f-8901-123456789018",
				"123c45d6-7e89-40f1-23a4-123456789019",
				"d5eade8b-11a4-4e26-b7e6-123456789020",
				"a9b0c1d2-34e5-46f7-8a9b-123456789021",
				"b3c4d5e6-7f89-4012-b3c4-123456789022",
				"c5d6e7f8-9012-4a3b-c5d6-123456789023",
				"e7f89012-3a4b-567c-e7f8-123456789024",
				"01a2b34c-5d6e-4890-1a2b-123456789025",
				"23c4d56e-78f9-40a1-23c4-123456789026",
				"45d6e7f8-9012-41a3-45d6-123456789027",
				"6789a0b1-c23d-4e56-6789-123456789028",
				"90a1b2c3-4d56-47e8-90a1-123456789029",
				"b3c4d5e6-7f89-4012-b3c4-123456789031",
				"e7f89012-3a4b-567c-e7f8-123456789032",
				"01a2b34c-5d6e-4890-1a2b-123456789033",
				"23c4d56e-78f9-40a1-23c4-123456789034",
				"45d6e7f8-9012-41a3-45d6-123456789035",
				"6789a0b1-c23d-4e56-6789-123456789036",
				"90a1b2c3-4d56-47e8-90a1-123456789037",
				"b3c4d5e6-7f89-4012-b3c4-123456789038",
				"e7f89012-3a4b-567c-e7f8-123456789039",
				"01a2b34c-5d6e-4890-1a2b-123456789040"
			};
			builder.HasData(
				new RateEntity
				{
					Id = 1,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 8.9,
					DriverId = 1,
					Description = "Great service!",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 2,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 6.4,
					DriverId = 2,
					Description = "Average ride experience.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 3,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 10.0,
					DriverId = 1,
					Description = "Excellent driver, highly recommended.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 4,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 5.6,
					DriverId = 4,
					Description = "Disappointing service, need improvements.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 5,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 9.4,
					DriverId = 1,
					Description = "Very friendly driver, enjoyed the ride.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 6,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 8.4,
					DriverId = 3,
					Description = "Smooth ride, no complaints.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 7,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 7.8,
					DriverId = 2,
					Description = "Could be better, but decent service.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 8,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 9.2,
					DriverId = 1,
					Description = "Professional driver, satisfied with the ride.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 9,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 6.7,
					DriverId = 4,
					Description = "Could improve punctuality.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 10,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 7.5,
					DriverId = 3,
					Description = "Decent ride overall.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 11,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 9.8,
					DriverId = 2,
					Description = "Highly professional driver, would ride again.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 12,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 5.1,
					DriverId = 4,
					Description = "Unsatisfactory service, needs improvement.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 13,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 9.7,
					DriverId = 1,
					Description = "Great ride experience!",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 14,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 7.3,
					DriverId = 3,
					Description = "Average service, nothing extraordinary.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 15,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 8.1,
					DriverId = 2,
					Description = "Good ride, no complaints.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 16,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 6.9,
					DriverId = 1,
					Description = "Could improve cleanliness.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 17,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 9.3,
					DriverId = 4,
					Description = "Pleasant and comfortable ride.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 18,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 7.7,
					DriverId = 3,
					Description = "Satisfactory service, would recommend.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 19,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 9.9,
					DriverId = 2,
					Description = "Exceptional ride, exceeded expectations.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 20,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 4.8,
					DriverId = 4,
					Description = "Terrible experience, would not recommend.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 21,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 9.6,
					DriverId = 1,
					Description = "Excellent service, very satisfied.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 22,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 7.2,
					DriverId = 3,
					Description = "Overall, a good experience.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 23,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 8.5,
					DriverId = 2,
					Description = "Polite and friendly driver.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 24,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 7.0,
					DriverId = 1,
					Description = "Average ride, nothing remarkable.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 25,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 9.1,
					DriverId = 4,
					Description = "Efficient service, no issues.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 26,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 8.0,
					DriverId = 3,
					Description = "Reliable driver, smooth ride.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 27,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 9.5,
					DriverId = 2,
					Description = "Outstanding service, highly recommended.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 28,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 4.3,
					DriverId = 4,
					Description = "Worst ride ever, avoid this driver.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 29,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 9.9,
					DriverId = 1,
					Description = "Exceptional ride, best driver I've had.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				},
				new RateEntity
				{
					Id = 30,
					UserId = userIds[GetRandomInt(0, 39)],
					Rate = 7.5,
					DriverId = 3,
					Description = "Decent service, no major complaints.",
					DateCreated = GetRandomDate(startDate, DateTime.Now, random)
				}


			);
		}
	}
}
