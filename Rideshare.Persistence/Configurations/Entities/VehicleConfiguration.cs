using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rideshare.Domain.Entities;

namespace Rideshare.Persistence.Configurations.Entities
{
    public class VehicleConfiguration : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasData(
                new Vehicle
                {
                    Id = 1,
                    PlateNumber = "ABC123",
                    NumberOfSeats = 40,
                    Model = "Toyota Camry",
                    Libre = "Libre 1",
                    DriverId = 1
                },
                new Vehicle
                {
                    Id = 2,
                    PlateNumber = "XYZ789",
                    NumberOfSeats = 5,
                    Model = "Honda Civic",
                    Libre = "Libre 2",
                    DriverId = 2
                },
                new Vehicle
                {
                    Id = 3,
                    PlateNumber = "DEF456",
                    NumberOfSeats = 7,
                    Model = "Ford Explorer",
                    Libre = "Libre 3",
                    DriverId = 3
                },
                new Vehicle
                {
                    Id = 4,
                    PlateNumber = "MNO789",
                    NumberOfSeats = 4,
                    Model = "Hyundai Elantra",
                    Libre = "Libre 4",
                    DriverId = 4
                },
                new Vehicle
                {
                    Id = 5,
                    PlateNumber = "PQR012",
                    NumberOfSeats = 6,
                    Model = "Nissan Altima",
                    Libre = "Libre 5",
                    DriverId = 5
                }
            );
        }
    }
}
