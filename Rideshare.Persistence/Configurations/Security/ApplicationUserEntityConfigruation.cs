using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Rideshare.Domain.Models;

namespace Rideshare.Persistence.Configurations.Security;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
	

   

	public void Configure(EntityTypeBuilder<ApplicationUser> builder)
	{

		var admin = new ApplicationUser
		{


			Id ="6f09dad5-2268-4410-b755-cf7859927e5f",
			UserName = "Admin",
			NormalizedUserName ="Admin".ToUpperInvariant(),
			Email = "temp@gmail.com",
			NormalizedEmail = "temp@gmail.com".ToUpperInvariant(),
			FullName="Admin",
			PhoneNumber="+2519393423022",

		};

		admin.PasswordHash = HashUserPassword(admin, "Admin@123RideShare");

		builder.HasData(admin);
		
		//More Users with Commuter and Driver Role
		builder.HasData(
				new ApplicationUser
				{
					Id = "2e4c2829-138d-4e7e-b023-123456789001",
					UserName = "Commuter1",
					NormalizedUserName = "Commuter1".ToUpperInvariant(),
					Email = "commuter1@gmail.com",
					NormalizedEmail = "commuter1@gmail.com".ToUpperInvariant(),
					FullName = "Jebesa Dejene",
					PhoneNumber = "+251911111111"
				},
				
				new ApplicationUser
				{
					Id = "2e4c2829-138d-4e7e-b023-123456789002",
					UserName = "Commuter2",
					NormalizedUserName = "Commuter2".ToUpperInvariant(),
					Email = "commuter2@gmail.com",
					NormalizedEmail = "commuter2@gmail.com".ToUpperInvariant(),
					FullName = "Abelras Mekonnen",
					PhoneNumber = "+251922222222"
				},
				
				new ApplicationUser
				{
					Id = "2e4c2829-138d-4e7e-b023-123456789003",
					UserName = "Commuter3",
					NormalizedUserName = "Commuter3".ToUpperInvariant(),
					Email = "commuter3@gmail.com",
					NormalizedEmail = "commuter3@gmail.com".ToUpperInvariant(),
					FullName = "Alemu kebede",
					PhoneNumber = "+251933333334"
				},
				new ApplicationUser
				{
					Id = "2e4c2829-138d-4e7e-b023-123456789004",
					UserName = "Commuter4",
					NormalizedUserName = "Commuter4".ToUpperInvariant(),
					Email = "commuter4@gmail.com",
					NormalizedEmail = "commuter4@gmail.com".ToUpperInvariant(),
					FullName = "Gizaw Dagne",
					PhoneNumber = "+251944444445"
				},
				
				new ApplicationUser
				{
					Id = "2e4c2829-138d-4e7e-b023-123456789005",
					UserName = "Commuter4",
					NormalizedUserName = "Commuter5".ToUpperInvariant(),
					Email = "commuter5@gmail.com",
					NormalizedEmail = "commuter5@gmail.com".ToUpperInvariant(),
					FullName = "Amha Kindu",
					PhoneNumber = "+251955555555"
				}
				
				
				
				// Add more commuter users here...
			);

			builder.HasData(
				new ApplicationUser
				{
					Id = "2e4c2829-138d-4e7e-b023-123456789101",
					UserName = "Driver1",
					NormalizedUserName = "Driver1".ToUpperInvariant(),
					Email = "driver1@gmail.com",
					NormalizedEmail = "driver1@gmail.com".ToUpperInvariant(),
					FullName = "Siyum Anammo",
					PhoneNumber = "+251933333333"
				},
				new ApplicationUser
				{
					Id = "2e4c2829-138d-4e7e-b023-123456789102",
					UserName = "Driver2",
					NormalizedUserName = "Driver2".ToUpperInvariant(),
					Email = "driver2@gmail.com",
					NormalizedEmail = "driver2@gmail.com".ToUpperInvariant(),
					FullName = "Kalu Siyum",
					PhoneNumber = "+251944444444"
				},
				
				new ApplicationUser
				{
					Id = "2e4c2829-138d-4e7e-b023-123456789103",
					UserName = "Driver3",
					NormalizedUserName = "Driver3".ToUpperInvariant(),
					Email = "driver3@gmail.com",
					NormalizedEmail = "driver3@gmail.com".ToUpperInvariant(),
					FullName = "Aster Nesu",
					PhoneNumber = "+251955555556"
				},
				
				new ApplicationUser
				{
					Id = "2e4c2829-138d-4e7e-b023-123456789104",
					UserName = "Driver4",
					NormalizedUserName = "Driver4".ToUpperInvariant(),
					Email = "driver4@gmail.com",
					NormalizedEmail = "driver4@gmail.com".ToUpperInvariant(),
					FullName = "Melkamu Yibeltal",
					PhoneNumber = "+251966666667"
				},
				new ApplicationUser
				{
					Id = "2e4c2829-138d-4e7e-b023-123456789105",
					UserName = "Driver4",
					NormalizedUserName = "Driver5".ToUpperInvariant(),
					Email = "driver5@gmail.com",
					NormalizedEmail = "driver5@gmail.com".ToUpperInvariant(),
					FullName = "Nurilgn Beyene",
					PhoneNumber = "+251933737377"
				}
				
				
				// Add more driver users here...
			);

		}
	

	private static string HashUserPassword(ApplicationUser user, string password)
	{
		var passHash = new PasswordHasher<ApplicationUser>();
		return passHash.HashPassword(user, password);
	}
}