using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace Rideshare.Persistence.Configurations.Security
{
    public class AppUserRoleConfig : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        private const string AdminRoleId = "bcaa5c92-d9d8-4106-8150-91cb40139030";
        private const string CommuterRoleId = "5970d313-8ead-434b-a1ea-cacbc6b5c0e9";
        private const string DriverRoleId = "9f4ca49c-f74f-4a97-b90c-b66f40eb9a5f";

        public AppUserRoleConfig()
        {
        }

        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = AdminRoleId,
                    UserId = "6f09dad5-2268-4410-b755-cf7859927e5f"
                },
                new IdentityUserRole<string>
                {
                    RoleId = CommuterRoleId,
                    UserId = "2e4c2829-138d-4e7e-b023-123456789001"
                },
                new IdentityUserRole<string>
                {
                    RoleId = CommuterRoleId,
                    UserId = "2e4c2829-138d-4e7e-b023-123456789002"
                },
                new IdentityUserRole<string>
                {
                    RoleId = CommuterRoleId,
                    UserId = "2e4c2829-138d-4e7e-b023-123456789003"
                },
                new IdentityUserRole<string>
                {
                    RoleId = CommuterRoleId,
                    UserId = "2e4c2829-138d-4e7e-b023-123456789004"
                },
                new IdentityUserRole<string>
                {
                    RoleId = CommuterRoleId,
                    UserId = "2e4c2829-138d-4e7e-b023-123456789005"
                },
                new IdentityUserRole<string>
                {
                    RoleId = DriverRoleId,
                    UserId = "2e4c2829-138d-4e7e-b023-123456789101"
                },
                new IdentityUserRole<string>
                {
                    RoleId = DriverRoleId,
                    UserId = "2e4c2829-138d-4e7e-b023-123456789102"
                },
                new IdentityUserRole<string>
                {
                    RoleId = DriverRoleId,
                    UserId = "2e4c2829-138d-4e7e-b023-123456789103"
                },
                new IdentityUserRole<string>
                {
                    RoleId = DriverRoleId,
                    UserId = "2e4c2829-138d-4e7e-b023-123456789104"
                },
                new IdentityUserRole<string>
                {
                    RoleId = DriverRoleId,
                    UserId = "2e4c2829-138d-4e7e-b023-123456789105"
                }
            );
        }
    }
}
