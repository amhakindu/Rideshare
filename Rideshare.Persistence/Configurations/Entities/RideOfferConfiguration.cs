using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetTopologySuite.Geometries;
using Rideshare.Domain.Common;
using Rideshare.Domain.Entities;

namespace Rideshare.Persistence.Configurations.Entities
{
	public class RideOfferConfiguration : IEntityTypeConfiguration<RideOffer>
	{
		public void Configure(EntityTypeBuilder<RideOffer> builder)
		{
			builder.HasData(
				new RideOffer
				{
					Id = 1,
					DriverId = 1,
					VehicleId = 1,
					CurrentLocationId = 1,
					DestinationId = 1,
					Status = Status.WAITING,
					AvailableSeats = 4,
					EstimatedFare = 27.0,
					DateCreated = new DateTime(2023, 1, 1),
					EstimatedDuration = TimeSpan.FromMinutes(50)
				},
				new RideOffer
				{
					Id = 2,
					DriverId = 2,
					VehicleId = 2,
					CurrentLocationId = 2,
					DestinationId = 2,
					Status = Status.COMPLETED,
					AvailableSeats = 3,
					EstimatedFare = 30.0,
					DateCreated = new DateTime(2023, 1, 2),
					EstimatedDuration = TimeSpan.FromMinutes(45)
				},
				new RideOffer
				{
					Id = 3,
					DriverId = 3,
					VehicleId = 3,
					CurrentLocationId = 3,
					DestinationId = 3,
					Status = Status.CANCELLED,
					AvailableSeats = 2,
					EstimatedFare = 25.0,
					DateCreated = new DateTime(2023, 1, 3),
					EstimatedDuration = TimeSpan.FromMinutes(55)
				},
				// Continue adding 20 more RideOffer entries
				new RideOffer
				{
					Id = 4,
					DriverId = 4,
					VehicleId = 4,
					CurrentLocationId = 4,
					DestinationId = 4,
					Status = Status.ONROUTE,
					AvailableSeats = 3,
					EstimatedFare = 32.0,
					DateCreated = new DateTime(2023, 1, 4),
					EstimatedDuration = TimeSpan.FromMinutes(40)
				},
				new RideOffer
				{
					Id = 5,
					DriverId = 5,
					VehicleId = 5,
					CurrentLocationId = 5,
					DestinationId = 5,
					Status = Status.WAITING,
					AvailableSeats = 1,
					EstimatedFare = 22.0,
					DateCreated = new DateTime(2023, 1, 5),
					EstimatedDuration = TimeSpan.FromMinutes(30)
				},
				
				
				new RideOffer
				{
					Id = 8,
					DriverId = 8,
					VehicleId = 8,
					CurrentLocationId = 8,
					DestinationId = 8,
					Status = Status.ONROUTE,
					AvailableSeats = 3,
					EstimatedFare = 30.0,
					DateCreated = new DateTime(2023, 1, 8),
					EstimatedDuration = TimeSpan.FromMinutes(45)
				},
				new RideOffer
				{
					Id = 9,
					DriverId = 9,
					VehicleId = 9,
					CurrentLocationId = 9,
					DestinationId = 9,
					Status = Status.WAITING,
					AvailableSeats = 2,
					EstimatedFare = 26.0,
					DateCreated = new DateTime(2023, 1, 9),
					EstimatedDuration = TimeSpan.FromMinutes(55)
				},
				new RideOffer
				{
					Id = 10,
					DriverId = 10,
					VehicleId = 10,
					CurrentLocationId = 10,
					DestinationId = 10,
					Status = Status.COMPLETED,
					AvailableSeats = 4,
					EstimatedFare = 27.0,
					DateCreated = new DateTime(2023, 1, 10),
					EstimatedDuration = TimeSpan.FromMinutes(40)
				},
				new RideOffer
				{
					Id = 11,
					DriverId = 1,
					VehicleId = 2,
					CurrentLocationId = 3,
					DestinationId = 4,
					Status = Status.CANCELLED,
					AvailableSeats = 3,
					EstimatedFare = 32.0,
					DateCreated = new DateTime(2023, 1, 11),
					EstimatedDuration = TimeSpan.FromMinutes(30)
				},
				new RideOffer
				{
					Id = 12,
					DriverId = 2,
					VehicleId = 3,
					CurrentLocationId = 4,
					DestinationId = 5,
					Status = Status.ONROUTE,
					AvailableSeats = 2,
					EstimatedFare = 24.0,
					DateCreated = new DateTime(2023, 1, 12),
					EstimatedDuration = TimeSpan.FromMinutes(50)
				},
				new RideOffer
				{
					Id = 13,
					DriverId = 3,
					VehicleId = 4,
					CurrentLocationId = 5,
					DestinationId = 6,
					Status = Status.WAITING,
					AvailableSeats = 1,
					EstimatedFare = 20.0,
					DateCreated = new DateTime(2023, 1, 13),
					EstimatedDuration = TimeSpan.FromMinutes(45)
				},
				new RideOffer
				{
					Id = 14,
					DriverId = 4,
					VehicleId = 5,
					CurrentLocationId = 6,
					DestinationId = 7,
					Status = Status.COMPLETED,
					AvailableSeats = 4,
					EstimatedFare = 27.0,
					DateCreated = new DateTime(2023, 1, 14),
					EstimatedDuration = TimeSpan.FromMinutes(55)
				},
				new RideOffer
				{
					Id = 15,
					DriverId = 5,
					VehicleId = 3,
					CurrentLocationId = 7,
					DestinationId = 8,
					Status = Status.CANCELLED,
					AvailableSeats = 3,
					EstimatedFare = 29.0,
					DateCreated = new DateTime(2023, 1, 15),
					EstimatedDuration = TimeSpan.FromMinutes(40)
				},
				
				new RideOffer
				{
					Id = 17,
					DriverId = 2,
					VehicleId = 8,
					CurrentLocationId = 9,
					DestinationId = 10,
					Status = Status.WAITING,
					AvailableSeats = 1,
					EstimatedFare = 23.0,
					DateCreated = new DateTime(2023, 1, 17),
					EstimatedDuration = TimeSpan.FromMinutes(50)
				},
				new RideOffer
				{
					Id = 18,
					DriverId = 8,
					VehicleId = 9,
					CurrentLocationId = 10,
					DestinationId = 1,
					Status = Status.COMPLETED,
					AvailableSeats = 4,
					EstimatedFare = 28.0,
					DateCreated = new DateTime(2023, 1, 18),
					EstimatedDuration = TimeSpan.FromMinutes(45)
				},
				new RideOffer
				{
					Id = 19,
					DriverId = 9,
					VehicleId = 10,
					CurrentLocationId = 1,
					DestinationId = 2,
					Status = Status.CANCELLED,
					AvailableSeats = 3,
					EstimatedFare = 30.0,
					DateCreated = new DateTime(2023, 1, 19),
					EstimatedDuration = TimeSpan.FromMinutes(55)
				},
				new RideOffer
				{
					Id = 20,
					DriverId = 10,
					VehicleId = 1,
					CurrentLocationId = 2,
					DestinationId = 3,
					Status = Status.ONROUTE,
					AvailableSeats = 2,
					EstimatedFare = 25.0,
					DateCreated = new DateTime(2023, 1, 20),
					EstimatedDuration = TimeSpan.FromMinutes(40)
				},
				new RideOffer
				{
					Id = 21,
					DriverId = 1,
					VehicleId = 2,
					CurrentLocationId = 3,
					DestinationId = 4,
					Status = Status.WAITING,
					AvailableSeats = 3,
					EstimatedFare = 27.0,
					DateCreated = new DateTime(2023, 1, 21),
					EstimatedDuration = TimeSpan.FromMinutes(30)
				},
				new RideOffer
				{
					Id = 22,
					DriverId = 2,
					VehicleId = 3,
					CurrentLocationId = 4,
					DestinationId = 5,
					Status = Status.COMPLETED,
					AvailableSeats = 2,
					EstimatedFare = 26.0,
					DateCreated = new DateTime(2023, 1, 22),
					EstimatedDuration = TimeSpan.FromMinutes(50)
				},
				new RideOffer
				{
					Id = 23,
					DriverId = 3,
					VehicleId = 4,
					CurrentLocationId = 5,
					DestinationId = 6,
					Status = Status.CANCELLED,
					AvailableSeats = 1,
					EstimatedFare = 20.0,
					DateCreated = new DateTime(2023, 1, 23),
					EstimatedDuration = TimeSpan.FromMinutes(45)
				},
				new RideOffer
				{
					Id = 24,
					DriverId = 4,
					VehicleId = 5,
					CurrentLocationId = 6,
					DestinationId = 7,
					Status = Status.ONROUTE,
					AvailableSeats = 4,
					EstimatedFare = 29.0,
					DateCreated = new DateTime(2023, 1, 24),
					EstimatedDuration = TimeSpan.FromMinutes(55)
				},
				new RideOffer
				{
					Id = 25,
					DriverId = 5,
					VehicleId = 3,
					CurrentLocationId = 7,
					DestinationId = 8,
					Status = Status.WAITING,
					AvailableSeats = 3,
					EstimatedFare = 27.0,
					DateCreated = new DateTime(2023, 1, 25),
					EstimatedDuration = TimeSpan.FromMinutes(40)
				},
			
				
				new RideOffer
				{
					Id = 28,
					DriverId = 8,
					VehicleId = 9,
					CurrentLocationId = 10,
					DestinationId = 1,
					Status = Status.ONROUTE,
					AvailableSeats = 4,
					EstimatedFare = 30.0,
					DateCreated = new DateTime(2023, 1, 28),
					EstimatedDuration = TimeSpan.FromMinutes(45)
				},
				new RideOffer
				{
					Id = 29,
					DriverId = 9,
					VehicleId = 10,
					CurrentLocationId = 1,
					DestinationId = 2,
					Status = Status.WAITING,
					AvailableSeats = 3,
					EstimatedFare = 26.0,
					DateCreated = new DateTime(2023, 1, 29),
					EstimatedDuration = TimeSpan.FromMinutes(55)
				},
				new RideOffer
				{
					Id = 30,
					DriverId = 10,
					VehicleId = 1,
					CurrentLocationId = 2,
					DestinationId = 3,
					Status = Status.COMPLETED,
					AvailableSeats = 2,
					EstimatedFare = 28.0,
					DateCreated = new DateTime(2023, 1, 30),
					EstimatedDuration = TimeSpan.FromMinutes(40)
				}
			);
		}
	}
}
