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
                    DestinationId = 2,
                    Status = Status.WAITING,
                    AvailableSeats = 3,
                    EstimatedFare = 20.0,
                    EstimatedDuration = TimeSpan.FromMinutes(30)
                },
                new RideOffer
                {
                    Id = 2,
                    DriverId = 2,
                    VehicleId = 2,
                    CurrentLocationId = 2,
                    DestinationId = 1,
                    Status = Status.WAITING,
                    AvailableSeats = 2,
                    EstimatedFare = 15.0,
                    EstimatedDuration = TimeSpan.FromMinutes(25)
                },
                new RideOffer
                {
                    Id = 3,
                    DriverId = 3,
                    VehicleId = 3,
                    CurrentLocationId = 3,
                    DestinationId = 4,
                    Status = Status.WAITING,
                    AvailableSeats = 4,
                    EstimatedFare = 25.0,
                    EstimatedDuration = TimeSpan.FromMinutes(40)
                },
                new RideOffer
                {
                    Id = 4,
                    DriverId = 4,
                    VehicleId = 4,
                    CurrentLocationId = 4,
                    DestinationId = 1,
                    Status = Status.WAITING,
                    AvailableSeats = 1,
                    EstimatedFare = 10.0,
                    EstimatedDuration = TimeSpan.FromMinutes(15)
                },
                new RideOffer
                {
                    Id = 5,
                    DriverId = 5,
                    VehicleId = 5,
                    CurrentLocationId = 1,
                    DestinationId = 2,
                    Status = Status.WAITING,
                    AvailableSeats = 2,
                    EstimatedFare = 18.0,
                    EstimatedDuration = TimeSpan.FromMinutes(35)
                }
            );
        }
    }
}
