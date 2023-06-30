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
			var userIds = new List<string>(){
				"d5eade8b-11a4-4e26-b7e6-123456789000",
				"123a8b56-c7d9-4e0a-89bf-123456789001",
				"456b8d23-e9f0-4c1d-ace6-123456789002",
				"789c0e12-3d45-4a6f-8b27-123456789003",
				"012d3aef-4b56-471c-86d9-123456789004",
				"345e6bcd-7890-4def-12ab-123456789005",
				"678f9de0-1c23-4ab4-5678-123456789006",
				"90a1b234-c5d6-4e7f-8901-123456789007",
				"12b3c4d5-67e8-490f-1a2b-123456789008",
				"34e5f6a7-8901-4bcd-23ef-123456789009",
				"5678a9b0-c1d2-4e3f-4a5b-123456789010",
				"8901c2d3-4e56-47f8-9012-123456789011",
				"1234e56f-7890-4c1d-23e4-123456789012",
				"4567f89a-0b12-43c4-5678-123456789013",
				"7890a1b2-34c5-4d6e-7890-123456789014",
				"0c1d23e4-56f7-4980-12ab-123456789015",
				"3e4f56a7-8901-4b2c-3d4e-123456789016",
				"6789a0b1-c23d-4e5f-6789-123456789017",
				"90a1b23c-45d6-4e7f-8901-123456789018",
				"123c45d6-7e89-40f1-23a4-123456789019",
				"d5eade8b-11a4-4e26-b7e6-123456789020",
				"a9b0c1d2-34e5-46f7-8a9b-123456789021",
				"b3c4d5e6-7f89-4012-b3c4-123456789022",
				"c5d6e7f8-9012-4a3b-c5d6-123456789023",
				"e7f89012-3a4b-567c-e7f8-123456789024",
				"01a2b34c-5d6e-4890-1a2b-123456789025",
				"23c4d56e-78f9-40a1-23c4-123456789026",
				"45d6e7f8-9012-41a3-45d6-123456789027",
				"6789a0b1-c23d-4e56-6789-123456789028",
				"90a1b2c3-4d56-47e8-90a1-123456789029",
				"b3c4d5e6-7f89-4012-b3c4-123456789031",
				"e7f89012-3a4b-567c-e7f8-123456789032",
				"01a2b34c-5d6e-4890-1a2b-123456789033",
				"23c4d56e-78f9-40a1-23c4-123456789034",
				"45d6e7f8-9012-41a3-45d6-123456789035",
				"6789a0b1-c23d-4e56-6789-123456789036",
				"90a1b2c3-4d56-47e8-90a1-123456789037",
				"b3c4d5e6-7f89-4012-b3c4-123456789038",
				"e7f89012-3a4b-567c-e7f8-123456789039",
				"01a2b34c-5d6e-4890-1a2b-123456789040"
			};
			for(int i = 1; i <= userIds.Count();i++)
			{
				string roleid = i%4 != 0 ? CommuterRoleId: DriverRoleId;
				builder.HasData(
					new IdentityUserRole<string>
					{
						RoleId = roleid,
						UserId = userIds[i-1]
					}
				);
			}
		}
	}
}