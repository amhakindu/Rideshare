
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rideshare.Domain.Models;

namespace Rideshare.Persistence.Configurations.Security;

public class ApplicationRoleEntityConfiguration : IEntityTypeConfiguration<ApplicationRole>
{
    private const string AdminRoleId = "bcaa5c92-d9d8-4106-8150-91cb40139030";
   
    private const string CommuterRoleId = "5970d313-8ead-434b-a1ea-cacbc6b5c0e9";


    private const string DriverRoleId = "9f4ca49c-f74f-4a97-b90c-b66f40eb9a5f";

    private const string Admin = "Admin";
    private const string Commuter = "Commuter";

    private const string Driver = "Driver";

   

    public void Configure(EntityTypeBuilder<ApplicationRole> builder)
    {
        var admin = new ApplicationRole
        {
            Id = AdminRoleId,
            Name = Admin,
            NormalizedName = Admin.ToUpperInvariant()
        };
        builder.HasData(admin);

        var commuter = new ApplicationRole
        {
            Id = CommuterRoleId,
            Name = Commuter,
            NormalizedName = Commuter.ToUpperInvariant()
        };
        builder.HasData(commuter);


        var driver = new ApplicationRole
        {
            Id = DriverRoleId,
            Name = Driver,
            NormalizedName = Driver.ToUpperInvariant()
        };
        builder.HasData(driver);

      
       }
}