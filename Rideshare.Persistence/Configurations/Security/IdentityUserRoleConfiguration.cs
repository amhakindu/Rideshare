using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;

namespace Rideshare.Persistence.Configurations.Security;

public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
{
    private const string AdminRoleId = "bcaa5c92-d9d8-4106-8150-91cb40139030";

    

    public IdentityUserRoleConfiguration()
    {
        
    }

    public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
    {
        var admin = new IdentityUserRole<string>
        {
            RoleId = AdminRoleId,
            UserId = "6f09dad5-2268-4410-b755-cf7859927e5f"
        };
        builder.HasData(admin);
    }
}