using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite.Geometries;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;

namespace Rideshare.Persistence.Configurations.Entities
{
	public class RideRequestConfiguration : IEntityTypeConfiguration<RideRequest>
	{
		public void Configure(EntityTypeBuilder<RideRequest> builder)
		{
			builder.HasData(
				new RideRequest
				{
					Id = 1,
					OriginId = 1,
					DestinationId = 2,
					CurrentFare = 50.0,
					NumberOfSeats = 2,
					UserId = "2e4c2829-138d-4e7e-b023-123456789001",
					Status = Status.WAITING,
					Accepted = false,
					MatchedRideId=null
				},
				new RideRequest
				{
					Id = 2,
					OriginId = 2,
					DestinationId = 4,
					CurrentFare = 40.9,
					NumberOfSeats = 1,
					UserId = "2e4c2829-138d-4e7e-b023-123456789002",
					Status = Status.WAITING,
					Accepted = false,
					MatchedRideId=3
				},
				new RideRequest
				{
					Id = 3,
					OriginId = 3,
					DestinationId = 1,
					CurrentFare = 30.9,
					NumberOfSeats = 3,
					UserId = "2e4c2829-138d-4e7e-b023-123456789003",
					Status = Status.WAITING,
					Accepted = false,
					MatchedRideId=3
				},
				new RideRequest
				{
					Id = 4,
					OriginId = 4,
					DestinationId = 3,
					CurrentFare = 20.0,
					NumberOfSeats = 1,
					UserId = "2e4c2829-138d-4e7e-b023-123456789004",
					Status = Status.WAITING,
					Accepted = false,
					MatchedRideId=2
				},
				new RideRequest
				{
					Id = 5,
					OriginId = 5,
					DestinationId = 2,
					CurrentFare = 23.50,
					NumberOfSeats = 2,
					UserId = "2e4c2829-138d-4e7e-b023-123456789005",
					Status = Status.WAITING,
					Accepted = false,
					MatchedRideId=1
				}
			);
		}
	}
}
