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
					PhoneNumber = "+251912334456",
					CreatedAt = new DateTime(2022, 9, 1).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				
				new ApplicationUser
				{
					Id = "2e4c2829-138d-4e7e-b023-123456789002",
					UserName = "Commuter2",
					NormalizedUserName = "Commuter2".ToUpperInvariant(),
					Email = "commuter2@gmail.com",
					NormalizedEmail = "commuter2@gmail.com".ToUpperInvariant(),
					FullName = "Abelras Mekonnen",
					PhoneNumber = "+251922222222",
					CreatedAt = new DateTime(2022, 5, 9).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"

				},
				
				new ApplicationUser
				{
					Id = "2e4c2829-138d-4e7e-b023-123456789003",
					UserName = "Commuter3",
					NormalizedUserName = "Commuter3".ToUpperInvariant(),
					Email = "commuter3@gmail.com",
					NormalizedEmail = "commuter3@gmail.com".ToUpperInvariant(),
					FullName = "Alemu kebede",
					PhoneNumber = "+251933333334",
					CreatedAt = new DateTime(2022, 12, 6).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "2e4c2829-138d-4e7e-b023-123456789004",
					UserName = "Commuter4",
					NormalizedUserName = "Commuter4".ToUpperInvariant(),
					Email = "commuter4@gmail.com",
					NormalizedEmail = "commuter4@gmail.com".ToUpperInvariant(),
					FullName = "Gizaw Dagne",
					PhoneNumber = "+251944444445",
					CreatedAt = new DateTime(2023, 1, 2).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				
				new ApplicationUser
				{
					Id = "2e4c2829-138d-4e7e-b023-123456789005",
					UserName = "Commuter4",
					NormalizedUserName = "Commuter5".ToUpperInvariant(),
					Email = "commuter5@gmail.com",
					NormalizedEmail = "commuter5@gmail.com".ToUpperInvariant(),
					FullName = "Amha Kindu",
					PhoneNumber = "+251955555555",
					CreatedAt = new DateTime(2023, 1, 2).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
					
				},
				new ApplicationUser
				{
					Id = "d5eade8b-11a4-4e26-b7e6-123456789000",
					UserName = "User1",
					NormalizedUserName = "USER1",
					Email = "user1@gmail.com",
					NormalizedEmail = "user1@gmail.com".ToUpperInvariant(),
					FullName = "Abebe Kebede",
					PhoneNumber = "+251911111101",
					CreatedAt = new DateTime(2023, 1, 1).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "123a8b56-c7d9-4e0a-89bf-123456789001",
					UserName = "User2",
					NormalizedUserName = "USER2",
					Email = "user2@gmail.com",
					NormalizedEmail = "user2@gmail.com".ToUpperInvariant(),
					FullName = "Meron Tadesse",
					PhoneNumber = "+251911111102",
					CreatedAt = new DateTime(2023, 1, 8).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "456b8d23-e9f0-4c1d-ace6-123456789002",
					UserName = "User3",
					NormalizedUserName = "USER3",
					Email = "user3@gmail.com",
					NormalizedEmail = "user3@gmail.com".ToUpperInvariant(),
					FullName = "Tewodros Alemayehu",
					PhoneNumber = "+251911111103",
					CreatedAt = new DateTime(2023, 1, 15).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "789c0e12-3d45-4a6f-8b27-123456789003",
					UserName = "User4",
					NormalizedUserName = "USER4",
					Email = "user4@gmail.com",
					NormalizedEmail = "user4@gmail.com".ToUpperInvariant(),
					FullName = "Zewdu Mengistu",
					PhoneNumber = "+251911111104",
					CreatedAt = new DateTime(2023, 1, 22).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "012d3aef-4b56-471c-86d9-123456789004",
					UserName = "User5",
					NormalizedUserName = "USER5",
					Email = "user5@gmail.com",
					NormalizedEmail = "user5@gmail.com".ToUpperInvariant(),
					FullName = "Bruk Assefa",
					PhoneNumber = "+251911111105",
					CreatedAt = new DateTime(2023, 1, 29).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "345e6bcd-7890-4def-12ab-123456789005",
					UserName = "User6",
					NormalizedUserName = "USER6",
					Email = "user6@gmail.com",
					NormalizedEmail = "user6@gmail.com".ToUpperInvariant(),
					FullName = "Ephrem Tekle",
					PhoneNumber = "+251911111106",
					CreatedAt = new DateTime(2023, 2, 5).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "678f9de0-1c23-4ab4-5678-123456789006",
					UserName = "User7",
					NormalizedUserName = "USER7",
					Email = "user7@gmail.com",
					NormalizedEmail = "user7@gmail.com".ToUpperInvariant(),
					FullName = "Yodit Gebremariam",
					PhoneNumber = "+251911111107",
					CreatedAt = new DateTime(2023, 2, 12).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "90a1b234-c5d6-4e7f-8901-123456789007",
					UserName = "User8",
					NormalizedUserName = "USER8",
					Email = "user8@gmail.com",
					NormalizedEmail = "user8@gmail.com".ToUpperInvariant(),
					FullName = "Dawit Gizaw",
					PhoneNumber = "+251911111108",
					CreatedAt = new DateTime(2023, 2, 19).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "12b3c4d5-67e8-490f-1a2b-123456789008",
					UserName = "User9",
					NormalizedUserName = "USER9",
					Email = "user9@gmail.com",
					NormalizedEmail = "user9@gmail.com".ToUpperInvariant(),
					FullName = "Hirut Mengiste",
					PhoneNumber = "+251911111109",
					CreatedAt = new DateTime(2023, 2, 26).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "34e5f6a7-8901-4bcd-23ef-123456789009",
					UserName = "User10",
					NormalizedUserName = "USER10",
					Email = "user10@gmail.com",
					NormalizedEmail = "user10@gmail.com".ToUpperInvariant(),
					FullName = "Tigist Bekele",
					PhoneNumber = "+251911111110",
					CreatedAt = new DateTime(2023, 3, 5).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "5678a9b0-c1d2-4e3f-4a5b-123456789010",
					UserName = "User11",
					NormalizedUserName = "USER11",
					Email = "user11@gmail.com",
					NormalizedEmail = "user11@gmail.com".ToUpperInvariant(),
					FullName = "Abel Mekonen",
					PhoneNumber = "+251911111111",
					CreatedAt = new DateTime(2023, 3, 12).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "8901c2d3-4e56-47f8-9012-123456789011",
					UserName = "User12",
					NormalizedUserName = "USER12",
					Email = "user12@gmail.com",
					NormalizedEmail = "user12@gmail.com".ToUpperInvariant(),
					FullName = "Eden Hailu",
					PhoneNumber = "+251911111112",
					CreatedAt = new DateTime(2023, 3, 19).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "1234e56f-7890-4c1d-23e4-123456789012",
					UserName = "User13",
					NormalizedUserName = "USER13",
					Email = "user13@gmail.com",
					NormalizedEmail = "user13@gmail.com".ToUpperInvariant(),
					FullName = "Solomon Wondimu",
					PhoneNumber = "+251911111113",
					CreatedAt = new DateTime(2023, 3, 26).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "4567f89a-0b12-43c4-5678-123456789013",
					UserName = "User14",
					NormalizedUserName = "USER14",
					Email = "user14@gmail.com",
					NormalizedEmail = "user14@gmail.com".ToUpperInvariant(),
					FullName = "Aster Gizaw",
					PhoneNumber = "+251911111114",
					CreatedAt = new DateTime(2023, 4, 2).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "7890a1b2-34c5-4d6e-7890-123456789014",
					UserName = "User15",
					NormalizedUserName = "USER15",
					Email = "user15@gmail.com",
					NormalizedEmail = "user15@gmail.com".ToUpperInvariant(),
					FullName = "Daniel Kassa",
					PhoneNumber = "+251911111115",
					CreatedAt = new DateTime(2023, 4, 9).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "0c1d23e4-56f7-4980-12ab-123456789015",
					UserName = "User16",
					NormalizedUserName = "USER16",
					Email = "user16@gmail.com",
					NormalizedEmail = "user16@gmail.com".ToUpperInvariant(),
					FullName = "Tsehay Tesfaye",
					PhoneNumber = "+251911111116",
					CreatedAt = new DateTime(2023, 4, 16).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "3e4f56a7-8901-4b2c-3d4e-123456789016",
					UserName = "User17",
					NormalizedUserName = "USER17",
					Email = "user17@gmail.com",
					NormalizedEmail = "user17@gmail.com".ToUpperInvariant(),
					FullName = "Kidist Teklu",
					PhoneNumber = "+251911111117",
					CreatedAt = new DateTime(2023, 4, 23).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "6789a0b1-c23d-4e5f-6789-123456789017",
					UserName = "User18",
					NormalizedUserName = "USER18",
					Email = "user18@gmail.com",
					NormalizedEmail = "user18@gmail.com".ToUpperInvariant(),
					FullName = "Yeshiwork Mamo",
					PhoneNumber = "+251911111118",
					CreatedAt = new DateTime(2023, 4, 30).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "90a1b23c-45d6-4e7f-8901-123456789018",
					UserName = "User19",
					NormalizedUserName = "USER19",
					Email = "user19@gmail.com",
					NormalizedEmail = "user19@gmail.com".ToUpperInvariant(),
					FullName = "Genet Assefa",
					PhoneNumber = "+251911111119",
					CreatedAt = new DateTime(2023, 5, 7).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "123c45d6-7e89-40f1-23a4-123456789019",
					UserName = "User20",
					NormalizedUserName = "USER20",
					Email = "user20@gmail.com",
					NormalizedEmail = "user20@gmail.com".ToUpperInvariant(),
					FullName = "Eyerusalem Kebede",
					PhoneNumber = "+251911111120",
					CreatedAt = new DateTime(2023, 5, 14).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "d5eade8b-11a4-4e26-b7e6-123456789020",
					UserName = "User21",
					NormalizedUserName = "USER21",
					Email = "user21@gmail.com",
					NormalizedEmail = "user21@gmail.com".ToUpperInvariant(),
					FullName = "Abiy Mekonnen",
					PhoneNumber = "+251911111121",
					CreatedAt = new DateTime(2023, 5, 21).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "a9b0c1d2-34e5-46f7-8a9b-123456789021",
					UserName = "User22",
					NormalizedUserName = "USER22",
					Email = "user22@gmail.com",
					NormalizedEmail = "user22@gmail.com".ToUpperInvariant(),
					FullName = "Hiwot Girma",
					PhoneNumber = "+251911111122",
					CreatedAt = new DateTime(2023, 5, 28).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "b3c4d5e6-7f89-4012-b3c4-123456789022",
					UserName = "User23",
					NormalizedUserName = "USER23",
					Email = "user23@gmail.com",
					NormalizedEmail = "user23@gmail.com".ToUpperInvariant(),
					FullName = "Fikadu Kebede",
					PhoneNumber = "+251911111123",
					CreatedAt = new DateTime(2023, 6, 4).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "c5d6e7f8-9012-4a3b-c5d6-123456789023",
					UserName = "User24",
					NormalizedUserName = "USER24",
					Email = "user24@gmail.com",
					NormalizedEmail = "user24@gmail.com".ToUpperInvariant(),
					FullName = "Belete Tadesse",
					PhoneNumber = "+251911111124",
					CreatedAt = new DateTime(2023, 6, 11).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "e7f89012-3a4b-567c-e7f8-123456789024",
					UserName = "User25",
					NormalizedUserName = "USER25",
					Email = "user25@gmail.com",
					NormalizedEmail = "user25@gmail.com".ToUpperInvariant(),
					FullName = "Meskerem Alemu",
					PhoneNumber = "+251911111125",
					CreatedAt = new DateTime(2023, 6, 18).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "01a2b34c-5d6e-4890-1a2b-123456789025",
					UserName = "User26",
					NormalizedUserName = "USER26",
					Email = "user26@gmail.com",
					NormalizedEmail = "user26@gmail.com".ToUpperInvariant(),
					FullName = "Selamawit Mengistu",
					PhoneNumber = "+251911111126",
					CreatedAt = new DateTime(2023, 6, 25).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "23c4d56e-78f9-40a1-23c4-123456789026",
					UserName = "User27",
					NormalizedUserName = "USER27",
					Email = "user27@gmail.com",
					NormalizedEmail = "user27@gmail.com".ToUpperInvariant(),
					FullName = "Hanna Gizaw",
					PhoneNumber = "+251911111127",
					CreatedAt = new DateTime(2023, 6, 30).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "45d6e7f8-9012-41a3-45d6-123456789027",
					UserName = "User28",
					NormalizedUserName = "USER28",
					Email = "user28@gmail.com",
					NormalizedEmail = "user28@gmail.com".ToUpperInvariant(),
					FullName = "Biruk Yohannes",
					PhoneNumber = "+251911111128",
					CreatedAt = new DateTime(2023, 7, 7).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "6789a0b1-c23d-4e56-6789-123456789028",
					UserName = "User29",
					NormalizedUserName = "USER29",
					Email = "user29@gmail.com",
					NormalizedEmail = "user29@gmail.com".ToUpperInvariant(),
					FullName = "Meron Teshome",
					PhoneNumber = "+251911111129",
					CreatedAt = new DateTime(2023, 7, 14).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "90a1b2c3-4d56-47e8-90a1-123456789029",
					UserName = "User30",
					NormalizedUserName = "USER30",
					Email = "user30@gmail.com",
					NormalizedEmail = "user30@gmail.com".ToUpperInvariant(),
					FullName = "Mintesinot Alemayehu",
					PhoneNumber = "+251911111130",
					CreatedAt = new DateTime(2023, 7, 21).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "b3c4d5e6-7f89-4012-b3c4-123456789031",
					UserName = "User31",
					NormalizedUserName = "USER31",
					Email = "user31@gmail.com",
					NormalizedEmail = "user31@gmail.com".ToUpperInvariant(),
					FullName = "Tadelech Getachew",
					PhoneNumber = "+251911111131",
					CreatedAt = new DateTime(2023, 7, 28).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "e7f89012-3a4b-567c-e7f8-123456789032",
					UserName = "User32",
					NormalizedUserName = "USER32",
					Email = "user32@gmail.com",
					NormalizedEmail = "user32@gmail.com".ToUpperInvariant(),
					FullName = "Ephrem Tekle",
					PhoneNumber = "+251911111132",
					CreatedAt = new DateTime(2023, 8, 4).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "01a2b34c-5d6e-4890-1a2b-123456789033",
					UserName = "User33",
					NormalizedUserName = "USER33",
					Email = "user33@gmail.com",
					NormalizedEmail = "user33@gmail.com".ToUpperInvariant(),
					FullName = "Hana Solomon",
					PhoneNumber = "+251911111133",
					CreatedAt = new DateTime(2023, 8, 11).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "23c4d56e-78f9-40a1-23c4-123456789034",
					UserName = "User34",
					NormalizedUserName = "USER34",
					Email = "user34@gmail.com",
					NormalizedEmail = "user34@gmail.com".ToUpperInvariant(),
					FullName = "Kalkidan Kebede",
					PhoneNumber = "+251911111134",
					CreatedAt = new DateTime(2023, 8, 18).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "45d6e7f8-9012-41a3-45d6-123456789035",
					UserName = "User35",
					NormalizedUserName = "USER35",
					Email = "user35@gmail.com",
					NormalizedEmail = "user35@gmail.com".ToUpperInvariant(),
					FullName = "Zeritu Tsegaye",
					PhoneNumber = "+251911111135",
					CreatedAt = new DateTime(2023, 8, 25).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "6789a0b1-c23d-4e56-6789-123456789036",
					UserName = "User36",
					NormalizedUserName = "USER36",
					Email = "user36@gmail.com",
					NormalizedEmail = "user36@gmail.com".ToUpperInvariant(),
					FullName = "Yonas Mulugeta",
					PhoneNumber = "+251911111136",
					CreatedAt = new DateTime(2023, 9, 1).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "90a1b2c3-4d56-47e8-90a1-123456789037",
					UserName = "User37",
					NormalizedUserName = "USER37",
					Email = "user37@gmail.com",
					NormalizedEmail = "user37@gmail.com".ToUpperInvariant(),
					FullName = "Sosina Asfaw",
					PhoneNumber = "+251911111137",
					CreatedAt = new DateTime(2023, 9, 8).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "b3c4d5e6-7f89-4012-b3c4-123456789038",
					UserName = "User38",
					NormalizedUserName = "USER38",
					Email = "user38@gmail.com",
					NormalizedEmail = "user38@gmail.com".ToUpperInvariant(),
					FullName = "Getahun Bekele",
					PhoneNumber = "+251911111138",
					CreatedAt = new DateTime(2023, 9, 15).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "e7f89012-3a4b-567c-e7f8-123456789039",
					UserName = "User39",
					NormalizedUserName = "USER39",
					Email = "user39@gmail.com",
					NormalizedEmail = "user39@gmail.com".ToUpperInvariant(),
					FullName = "Mekdes Arega",
					PhoneNumber = "+251911111139",
					CreatedAt = new DateTime(2023, 9, 22).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "01a2b34c-5d6e-4890-1a2b-123456789040",
					UserName = "User40",
					NormalizedUserName = "USER40",
					Email = "user40@gmail.com",
					NormalizedEmail = "user40@gmail.com".ToUpperInvariant(),
					FullName = "Befikadu Tilahun",
					PhoneNumber = "+251911111140",
					CreatedAt = new DateTime(2023, 9, 29).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
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
					PhoneNumber = "+251933333333",
					CreatedAt = new DateTime(2023, 2, 6).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "2e4c2829-138d-4e7e-b023-123456789102",
					UserName = "Driver2",
					NormalizedUserName = "Driver2".ToUpperInvariant(),
					Email = "driver2@gmail.com",
					NormalizedEmail = "driver2@gmail.com".ToUpperInvariant(),
					FullName = "Kalu Siyum",
					PhoneNumber = "+251944444444",
					CreatedAt = new DateTime(2023, 3, 16).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				
				new ApplicationUser
				{
					Id = "2e4c2829-138d-4e7e-b023-123456789103",
					UserName = "Driver3",
					NormalizedUserName = "Driver3".ToUpperInvariant(),
					Email = "driver3@gmail.com",
					NormalizedEmail = "driver3@gmail.com".ToUpperInvariant(),
					FullName = "Aster Nesu",
					PhoneNumber = "+251955555556",
					CreatedAt = new DateTime(2023, 3, 11).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				
				new ApplicationUser
				{
					Id = "2e4c2829-138d-4e7e-b023-123456789104",
					UserName = "Driver4",
					NormalizedUserName = "Driver4".ToUpperInvariant(),
					Email = "driver4@gmail.com",
					NormalizedEmail = "driver4@gmail.com".ToUpperInvariant(),
					FullName = "Melkamu Yibeltal",
					PhoneNumber = "+251966666667",
					CreatedAt = new DateTime(2023, 4, 1).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
				},
				new ApplicationUser
				{
					Id = "2e4c2829-138d-4e7e-b023-123456789105",
					UserName = "Driver4",
					NormalizedUserName = "Driver5".ToUpperInvariant(),
					Email = "driver5@gmail.com",
					NormalizedEmail = "driver5@gmail.com".ToUpperInvariant(),
					FullName = "Nurilgn Beyene",
					PhoneNumber = "+251933737377",
					CreatedAt = new DateTime(2023, 5, 6).ToUniversalTime(),
					ProfilePicture = "http://res.cloudinary.com/dqy2ctugs/raw/upload/v1688115133/mteobaov9ac170lti8a1.png"
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