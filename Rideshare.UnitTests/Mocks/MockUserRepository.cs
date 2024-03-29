using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Rideshare.Application.Common.Dtos.Security;
using Rideshare.Application.Common.Dtos.Statistics;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Domain.Models;

namespace Rideshare.Application.UnitTests.Mocks
{
	public class MockUserRepository : Mock<IUserRepository>
	{
		public MockUserRepository()
		{
		 
			SetupMethods();
		}

		private void SetupMethods()
		{
			// FindByEmailAsync
			Setup(repo => repo.FindByEmailAsync(It.IsAny<string>()))
				.ReturnsAsync((string email) => GetUserByEmailAddress(email));

			// FindByIdAsync
			Setup(repo => repo.FindByIdAsync(It.IsAny<string>()))
				.ReturnsAsync((string userId) => GetUserById(userId));

			// CreateUserAsync
			Setup(repo => repo.CreateUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<List<ApplicationRole>>()))
				.ReturnsAsync((ApplicationUser user, List<ApplicationRole> roles) =>
				{
					// Assign unique identifier to the user and return the user object
					user.Id = GetUniqueIdentifier();
					return user;
				});

			// CheckPasswordAsync
			Setup(repo => repo.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
				.ReturnsAsync((ApplicationUser user, string password) =>
				{
					// Simulate password check logic here and return a boolean result
					return user.PasswordHash == HashPassword(password);
				});

			// UpdateUserAsync
			Setup(repo => repo.UpdateUserAsync(It.IsAny<string>(), It.IsAny<ApplicationUser>()))
				.ReturnsAsync((string userId, ApplicationUser user) =>
				{
					// Find the existing user by ID and update the user object
					var existingUser = GetUserById(userId);
					if (existingUser != null)
					{
						existingUser.FullName = user.FullName;
						existingUser.UserName = user.FullName;
						return existingUser;
					}
					return null;
				});

			// GetUsersAsync
			// Setup(repo => repo.GetUsersAsync())
			//     .ReturnsAsync(GetAllUsers());

			// DeleteUserAsync
			Setup(repo => repo.DeleteUserAsync(It.IsAny<string>()))
				.Returns(Task.CompletedTask);

			// CheckEmailExistence
			Setup(repo => repo.CheckEmailExistence(It.IsAny<string>(), It.IsAny<string>()))
				.ReturnsAsync((string email, string userId) =>
				{
					var user = GetUserByEmailAddress(email);
					return user != null && user.Id != userId;
				});

			// GetUserById
			Setup(repo => repo.GetUserById(It.IsAny<string>()))
				.ReturnsAsync((string userId) => GetUserById(userId));

			// ResetPassword
			Setup(repo => repo.ResetPassword(It.IsAny<string>(), It.IsAny<string>()))
				.Returns(Task.CompletedTask);

			// LoginAsync
			Setup(repo => repo.LoginAsync(It.IsAny<string>()))
				.ReturnsAsync((string phoneNumber) =>
				{
					var user = GetUserByPhoneNumber(phoneNumber);
					if (user != null)
					{
						
						var tokenDto = new TokenDto("access-token", "refresh-token");
						var loginResponse = new LoginResponse("Login successful", tokenDto.AccessToken, tokenDto.RefreshToken);
						return loginResponse;
					}
					return null;
				});

			// RefreshToken
			Setup(repo => repo.RefreshToken(It.IsAny<TokenDto>()))
				.ReturnsAsync((TokenDto tokenDto) =>
				{

					var refreshedTokenDto = new TokenDto("refreshed-access-token", "refreshed-refresh-token");
					return refreshedTokenDto;
				});
			
			Setup(repo => repo.GetCommuterStatistics(It.IsAny<int?>(), It.IsAny<int?>()))
				.ReturnsAsync((int? year, int? month) =>
				{
				var entities = new List<ApplicationUser> {
					new ApplicationUser { Id = "user1", CreatedAt = new DateTime(2022, 7, 1) },
					new ApplicationUser { Id = "user2", CreatedAt = new DateTime(2023, 7, 1) },
					new ApplicationUser { Id = "user3", CreatedAt = new DateTime(2023, 7, 5) },
					new ApplicationUser { Id = "user4", CreatedAt = new DateTime(2023, 7, 10) },
					new ApplicationUser { Id = "user5", CreatedAt = new DateTime(2023, 7, 16) },
					new ApplicationUser { Id = "user6", CreatedAt = new DateTime(2023, 8, 1) }
				};
			
				if(month != null && year != null){
					// Weekly
					var temp = entities.Where(entity => entity.CreatedAt.Year == year)
						.Where(entity => entity.CreatedAt.Month == month)
						.GroupBy(entity => entity.CreatedAt.Day / 7 + 1)
						.ToDictionary(group => group.Key, group => group.Count());
					for (int i = 1; i <= 5; i++)
					{
						if(!temp.ContainsKey(i))
							temp.Add(i, 0);
					}
					return temp;
				}else if(month == null && year == null){
				// Yearly
					var temp = entities
						.GroupBy(entity => entity.CreatedAt.Year)
						.ToDictionary(g => g.Key, g => g.Count());
					for (int i = 2023; i <= DateTime.Now.Year; i++)
					{
						if(!temp.ContainsKey(i))
							temp.Add(i, 0);
					}
					return temp;
				}else{   
					// Monthly
					Dictionary<int, int> temp = entities
						.Where(entity => entity.CreatedAt.Year == year)
						.GroupBy(entity => entity.CreatedAt.Month)
						.ToDictionary(g => g.Key, g => g.Count());
					for (int i = 1; i < 13; i++)
					{
						if(!temp.ContainsKey(i))
							temp.Add(i, 0);
					}
				return temp;
				}});
			
			
			Setup(repo => repo.GetCommuterStatusCountAsync())
				.ReturnsAsync(() =>
				{
				var commuters = new List<ApplicationUser>
					{
						new ApplicationUser
						{
							Id = "user1",
							FullName = "John Doe",
							UserName = "johndoe",
							Email = "john@example.com",
							PhoneNumber = "1234567890",
							LastLogin = DateTime.Now.AddDays(-15)
						},
						new ApplicationUser
						{
							Id = "user2",
							FullName = "Jane Smith",
							UserName = "janesmith",
							Email = "jane@example.com",
							PhoneNumber = "9876543210",
							LastLogin = DateTime.Now.AddDays(-45)
						}
					};
					
				int activeCommuters = 0;
				int idleCommuters = 0;

				if (commuters?.Count() > 0)
				{
					var thirtyDaysAgo = DateTime.Now.AddDays(-30);
					activeCommuters = commuters.Count(u => u.LastLogin >= thirtyDaysAgo);
					idleCommuters = commuters.Count(u => u.LastLogin < thirtyDaysAgo);
				}

				return new StatusDto
					{
						Active= activeCommuters,
						Idle = idleCommuters
					};
				});
			
		}
		

	 
		private string GetUniqueIdentifier()
		{
			return Guid.NewGuid().ToString();
		}

	   
		private ApplicationUser GetUserByEmailAddress(string email)
		{
		   
			return GetAllUsers().FirstOrDefault(u => u.Email == email);
		}

		// Helper method to retrieve a user by ID
		private ApplicationUser GetUserById(string userId)
		{
			// Simulate database query logic to retrieve a user by ID
			return GetAllUsers().FirstOrDefault(u => u.Id == userId);
		}

		// Helper method to retrieve a user by phone number
		private ApplicationUser GetUserByPhoneNumber(string phoneNumber)
		{
			// Simulate database query logic to retrieve a user by phone number
			return GetAllUsers().FirstOrDefault(u => u.PhoneNumber == phoneNumber);
		}

	
		
        // Helper method to retrieve all users
        private List<ApplicationUser> GetAllUsers()
        {
            // Simulate database query logic to retrieve all users
            return new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = "user1",
                    FullName = "John Doe",
                    UserName = "johndoe",
                    Email = "john@example.com",
                    PhoneNumber = "1234567890",
                    PasswordHash = HashPassword("password1"),
                    LastLogin = DateTime.Now.AddDays(-32),
                
                },
                new ApplicationUser
                {
                    Id = "user2",
                    FullName = "Jane Smith",
                    UserName = "janesmith",
                    Email = "jane@example.com",
                    PhoneNumber = "9876543210",
                    PasswordHash = HashPassword("password2"),
                    LastLogin = DateTime.Now.AddDays(-2)
                },
				new ApplicationUser
                {
                    Id = "user3",
                    FullName = "Jane Schmidt",
                    UserName = "janeschmidt",
                    Email = "janeschmidt@example.com",
                    PhoneNumber = "9866543210",
                    PasswordHash = HashPassword("password3"),
                    LastLogin = DateTime.Now.AddDays(-34)
                }
                // Add more user objects as needed for testing different scenarios
            };
        }

		// Helper method to simulate password hashing
		private string HashPassword(string password)
		{
			// Simulate password hashing logic here
			return password; // In this example, we're just returning the plain password
		}
	}
}
