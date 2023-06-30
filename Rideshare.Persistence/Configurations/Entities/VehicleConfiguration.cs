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
                    Libre = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688100796/xsabqbbff2jc7kqc6efx.pdf",
                    DriverId = 1
                },
                new Vehicle
                {
                    Id = 2,
                    PlateNumber = "XYZ789",
                    NumberOfSeats = 5,
                    Model = "Honda Civic",
                    Libre =  "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688100780/m39jk106ug96qt7e13me.pdf",
                    DriverId = 2
                },
                new Vehicle
                {
                    Id = 3,
                    PlateNumber = "DEF456",
                    NumberOfSeats = 7,
                    Model = "Ford Explorer",
                    Libre = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688100768/jo9dzde295c0ebkjr7hz.pdf",
                    DriverId = 3
                },
                new Vehicle
                {
                    Id = 4,
                    PlateNumber = "MNO789",
                    NumberOfSeats = 4,
                    Model = "Hyundai Elantra",
                    Libre =  "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688100754/xp7cy2ltzynyrkwlufzp.pdf",
                    DriverId = 4
                },
                new Vehicle
                {
                    Id = 5,
                    PlateNumber = "PQR012",
                    NumberOfSeats = 6,
                    Model = "Nissan Altima",
                    Libre =  "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688100699/iofw5wmwav58y9pems5w.pdf",
                    DriverId = 5
                }
            );
        }
    }
}
