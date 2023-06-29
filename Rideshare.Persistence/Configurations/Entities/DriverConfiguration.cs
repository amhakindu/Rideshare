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
                    License = "License 1"
                },
                new Driver
                {
                    Id = 2,
                    UserId = "2e4c2829-138d-4e7e-b023-123456789104",
                    Experience = 2.0,
                    Verified = false,
                    Address = "Arat kilo",
                    LicenseNumber = "87654321",
                    License = "License 2"
                },
                new Driver
                {
                    Id = 3,
                    UserId = "2e4c2829-138d-4e7e-b023-123456789103",
                    Experience = 4.8,
                    Verified = true,
                    Address = "Debrezeyit",
                    LicenseNumber = "98765432",
                    License = "License 3"
                },
                new Driver
                {
                    Id = 4,
                    UserId = "2e4c2829-138d-4e7e-b023-123456789102",
                    Experience = 1.2,
                    Verified = false,
                    Address = "Nazrit",
                    LicenseNumber = "23456789",
                    License = "License 4"
                },
                new Driver
                {
                    Id = 5,
                    UserId = "2e4c2829-138d-4e7e-b023-123456789101",
                    Experience = 5.0,
                    Verified = true,
                    Address = "Weraileu",
                    LicenseNumber = "98765432",
                    License = "License 5"
                }
            );
        }
    }
}
