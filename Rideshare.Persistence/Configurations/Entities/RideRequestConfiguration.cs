using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite.Geometries;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;

namespace Rideshare.Persistence.Configurations.Entities
{
	public class RideRequestConfiguration : IEntityTypeConfiguration<RideRequest>
	{
		DateTime GetRandomDate(DateTime start, DateTime end, Random random)
		{
			var range = (end - start).Days;
			return start.AddDays(random.Next(range));
		}
		public int GetRandomInt(int start, int end)
		{
			Random random = new Random();
			return random.Next(start, end + 1);
		}
		
		public void Configure(EntityTypeBuilder<RideRequest> builder)
		{
			var random = new Random();
			var startDate = new DateTime(2023, 1, 1).ToUniversalTime();
			var endDate = DateTime.UtcNow;
			
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
				new RideRequest
				{
					Id = 1,
					OriginId = 1,
					DestinationId = 2,
					CurrentFare = 50.0,
					NumberOfSeats = 2,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.WAITING,
					Accepted = false,
					MatchedRideId = null,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 2,
					OriginId = 2,
					DestinationId = 3,
					CurrentFare = 40.0,
					NumberOfSeats = 1,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.COMPLETED,
					Accepted = true,
					MatchedRideId = 1,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 3,
					OriginId = 3,
					DestinationId = 4,
					CurrentFare = 35.0,
					NumberOfSeats = 1,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.CANCELLED,
					Accepted = false,
					MatchedRideId = null,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 4,
					OriginId = 4,
					DestinationId = 5,
					CurrentFare = 45.0,
					NumberOfSeats = 3,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.ONROUTE,
					Accepted = true,
					MatchedRideId = 2,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 5,
					OriginId = 5,
					DestinationId = 6,
					CurrentFare = 30.0,
					NumberOfSeats = 1,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.WAITING,
					Accepted = false,
					MatchedRideId = null,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 6,
					OriginId = 6,
					DestinationId = 7,
					CurrentFare = 50.0,
					NumberOfSeats = 2,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.CANCELLED,
					Accepted = false,
					MatchedRideId = null,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 7,
					OriginId = 7,
					DestinationId = 8,
					CurrentFare = 40.0,
					NumberOfSeats = 1,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.COMPLETED,
					Accepted = true,
					MatchedRideId = 3,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 8,
					OriginId = 8,
					DestinationId = 9,
					CurrentFare = 35.0,
					NumberOfSeats = 1,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.WAITING,
					Accepted = false,
					MatchedRideId = null,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 9,
					OriginId = 9,
					DestinationId = 10,
					CurrentFare = 45.0,
					NumberOfSeats = 3,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.ONROUTE,
					Accepted = true,
					MatchedRideId = 4,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 10,
					OriginId = 10,
					DestinationId = 1,
					CurrentFare = 30.0,
					NumberOfSeats = 1,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.CANCELLED,
					Accepted = false,
					MatchedRideId = null,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 11,
					OriginId = 1,
					DestinationId = 2,
					CurrentFare = 50.0,
					NumberOfSeats = 2,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.WAITING,
					Accepted = false,
					MatchedRideId = null,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 12,
					OriginId = 2,
					DestinationId = 3,
					CurrentFare = 40.0,
					NumberOfSeats = 1,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.COMPLETED,
					Accepted = true,
					MatchedRideId = 5,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 13,
					OriginId = 3,
					DestinationId = 4,
					CurrentFare = 35.0,
					NumberOfSeats = 1,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.WAITING,
					Accepted = false,
					MatchedRideId = null,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 14,
					OriginId = 4,
					DestinationId = 5,
					CurrentFare = 45.0,
					NumberOfSeats = 3,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.ONROUTE,
					Accepted = true,
					MatchedRideId = 3,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 15,
					OriginId = 5,
					DestinationId = 6,
					CurrentFare = 30.0,
					NumberOfSeats = 1,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.COMPLETED,
					Accepted = true,
					MatchedRideId = 1,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 16,
					OriginId = 6,
					DestinationId = 7,
					CurrentFare = 50.0,
					NumberOfSeats = 2,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.CANCELLED,
					Accepted = false,
					MatchedRideId = null,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 17,
					OriginId = 7,
					DestinationId = 8,
					CurrentFare = 40.0,
					NumberOfSeats = 1,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.WAITING,
					Accepted = false,
					MatchedRideId = null,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 18,
					OriginId = 8,
					DestinationId = 9,
					CurrentFare = 35.0,
					NumberOfSeats = 1,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.COMPLETED,
					Accepted = true,
					MatchedRideId = 8,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 19,
					OriginId = 9,
					DestinationId = 10,
					CurrentFare = 45.0,
					NumberOfSeats = 3,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.ONROUTE,
					Accepted = true,
					MatchedRideId = 9,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 20,
					OriginId = 10,
					DestinationId = 1,
					CurrentFare = 30.0,
					NumberOfSeats = 1,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.WAITING,
					Accepted = false,
					MatchedRideId = null,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 21,
					OriginId = 1,
					DestinationId = 2,
					CurrentFare = 50.0,
					NumberOfSeats = 2,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.WAITING,
					Accepted = false,
					MatchedRideId = null,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 22,
					OriginId = 2,
					DestinationId = 3,
					CurrentFare = 40.0,
					NumberOfSeats = 1,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.COMPLETED,
					Accepted = true,
					MatchedRideId = 10,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 23,
					OriginId = 3,
					DestinationId = 4,
					CurrentFare = 35.0,
					NumberOfSeats = 1,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.WAITING,
					Accepted = false,
					MatchedRideId = null,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 24,
					OriginId = 4,
					DestinationId = 5,
					CurrentFare = 45.0,
					NumberOfSeats = 3,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.ONROUTE,
					Accepted = true,
					MatchedRideId = 11,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 25,
					OriginId = 5,
					DestinationId = 6,
					CurrentFare = 30.0,
					NumberOfSeats = 1,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.COMPLETED,
					Accepted = true,
					MatchedRideId = 12,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 26,
					OriginId = 6,
					DestinationId = 7,
					CurrentFare = 50.0,
					NumberOfSeats = 2,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.CANCELLED,
					Accepted = false,
					MatchedRideId = null,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 27,
					OriginId = 7,
					DestinationId = 8,
					CurrentFare = 40.0,
					NumberOfSeats = 1,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.WAITING,
					Accepted = false,
					MatchedRideId = null,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 28,
					OriginId = 8,
					DestinationId = 9,
					CurrentFare = 35.0,
					NumberOfSeats = 1,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.COMPLETED,
					Accepted = true,
					MatchedRideId = 13,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 29,
					OriginId = 9,
					DestinationId = 10,
					CurrentFare = 45.0,
					NumberOfSeats = 3,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.ONROUTE,
					Accepted = true,
					MatchedRideId = 14,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				},
				new RideRequest
				{
					Id = 30,
					OriginId = 10,
					DestinationId = 1,
					CurrentFare = 30.0,
					NumberOfSeats = 1,
					UserId = userIds[GetRandomInt(0, 39)],
					Status = Status.WAITING,
					Accepted = false,
					MatchedRideId = null,
					DateCreated = GetRandomDate(startDate, endDate, random),
					LastModifiedDate= GetRandomDate(startDate, DateTime.UtcNow, random)
				}
			);
		}
	}
}
