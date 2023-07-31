using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Rideshare.Domain.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Rideshare.Application.Responses;
using Microsoft.Extensions.Configuration;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Common.Dtos.Security;


namespace Rideshare.Persistence.Repositories.User;

public class UserRepository : IUserRepository
{
	private readonly UserManager<ApplicationUser> _userManager;

	private readonly RoleManager<IdentityRole> _roleManager;
	private readonly IJwtService _jwtService;
	private readonly IConfiguration _configuration;

	public UserRepository(UserManager<ApplicationUser> userManager, IConfiguration configuration, IJwtService jwtService)
	{
		_userManager = userManager;
		_configuration = configuration;
		_jwtService = jwtService;
	}

	public async Task<ApplicationUser> FindByEmailAsync(string email)
	{
		return await _userManager.FindByEmailAsync(email);
	}

	public async Task<ApplicationUser> FindByIdAsync(string userId)
	{
		return await _userManager.FindByIdAsync(userId);
	}

	public async Task<PaginatedResponse<ApplicationUser>> GetUsersByRoleAsync(string role, int pageNumber = 1,
	   int pageSize = 10)
	{
		var usersInRole = await _userManager.GetUsersInRoleAsync(role);
		var filteredUsers = usersInRole
		  .Skip((pageNumber - 1) * pageSize)
		  .Take(pageSize)
		  .ToList();
		var paginatedResponse = new PaginatedResponse<ApplicationUser>
		{
			Value= filteredUsers,
			Count = usersInRole.Count

		};
		return paginatedResponse;
	}

	public async Task<List<ApplicationRole>> GetUserRolesAsync(ApplicationUser? user)
	{
		var roles = (await _userManager.GetRolesAsync(user)).Select(r => new ApplicationRole
		{
			Name = r
		});

		return roles.ToList();
	}



	public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user, List<ApplicationRole> roles)
	{

		user.UserName = user.FullName.Replace(" ", "") + user.PhoneNumber;
		user.Email = user.FullName + user.PhoneNumber;
		var result = await _userManager.CreateAsync(user);
		Console.WriteLine(result.ToString());

		if (result.Succeeded)
		{


			var savedUser = await _userManager.Users.SingleOrDefaultAsync(us => us.PhoneNumber == user.PhoneNumber);

			var addRoleResult = await _userManager.AddToRolesAsync(user, roles.Select(r => r.Name));
			Console.WriteLine(addRoleResult.ToString());

			if (!addRoleResult.Succeeded) throw new InvalidOperationException("User role assignment has failed");


			return user;
		}
		else
		{
			Console.WriteLine(result.ToString());
			throw new Exception($"Failed to create user. {result.ToString()}");
		}
	}

	public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
	{
		return await _userManager.CheckPasswordAsync(user, password);
	}

	public async Task<ApplicationUser> UpdateUserAsync(string userId, ApplicationUser user)
	{
		var existingUser = await _userManager.FindByIdAsync(userId);
		if (existingUser == null)
		{
			throw new Exception("User not found.");
		}
		existingUser.FullName = user.FullName;
		existingUser.UserName = user.FullName;

		var result = await _userManager.UpdateAsync(existingUser);
		if (result.Succeeded)
		{
			return existingUser;
		}
		else
		{
			throw new Exception(result.ToString());
		}
	}

	public async Task<PaginatedResponse<ApplicationUser>> GetUsersAsync(int pageNumber = 1,
		int pageSize = 10)
	{
		var count = await _userManager.Users.CountAsync();
		var filteredUsers = _userManager.Users.
			Skip((pageNumber - 1) * pageSize)
			.Take(pageSize)
			.ToList();
		var paginatedResponse = new PaginatedResponse<ApplicationUser>
		{
			Value = filteredUsers,
			Count = count
		};
		return paginatedResponse;

	}

	public async Task DeleteUserAsync(string userId)
	{
		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
		{
			throw new Exception("User not found.");
		}

		var result = await _userManager.DeleteAsync(user);
		if (!result.Succeeded)
		{
			throw new Exception(result.ToString());
		}
	}

	public async Task<bool> CheckEmailExistence(string email, string? userId)
	{
		var user = await _userManager.FindByEmailAsync(email);
		return user != null && user.Id != userId;
	}

	public async Task<ApplicationUser> GetUserById(string userId)
	{
		return await _userManager.FindByIdAsync(userId);
	}

	public async Task ResetPassword(string userId, string password)
	{
		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
		{
			throw new Exception("User not found.");
		}

		var token = await _userManager.GeneratePasswordResetTokenAsync(user);
		var result = await _userManager.ResetPasswordAsync(user, token, password);
		if (!result.Succeeded)
		{
			throw new Exception(result.ToString());
		}
	}

	public async Task<LoginResponse> LoginAsync(string phoneNumber)
	{
		var user = await _userManager.Users.SingleOrDefaultAsync(us => us.PhoneNumber == phoneNumber);
		if (user == null)
		{
			throw new Exception("Invalid phoneNumber.");
		}
		var tokens = await _jwtService.GenerateToken(user);
		return new LoginResponse("Login successful", tokens.AccessToken, tokens.RefreshToken);


		;
	}


	public async Task<TokenDto?> RefreshToken(TokenDto tokenDto)
	{
		return await _jwtService.RefreshToken(tokenDto);
	}
	public async Task<LoginResponse> LoginByAdminAsync(string username, string password)
	{
		var user = await _userManager.FindByNameAsync(username);
		if (user == null)
		{
			throw new Exception("Invalid username or password.");
		}

		var validPassword = await _userManager.CheckPasswordAsync(user, password);
		if (!validPassword)
		{
			throw new Exception("Invalid username or password.");
		}

		var tokens = await _jwtService.GenerateToken(user);
		return new LoginResponse("Login successful", tokens.AccessToken, tokens.RefreshToken);


		;
	}
	public async Task<ApplicationUser> UpdateAdminUserAsync(string userId, ApplicationUser user)
	{
		var existingUser = await _userManager.FindByIdAsync(userId);
		if (existingUser == null)
		{
			throw new Exception("User not found.");
		}

		existingUser.Email = user.Email;
		existingUser.FullName = user.FullName;


		var result = await _userManager.UpdateAsync(existingUser);
		if (result.Succeeded)
		{
			return existingUser;
		}
		else
		{
			throw new Exception(result.Errors.ToString());
		}
	}

	public async Task<ApplicationUser> CreateAdminUserAsync(ApplicationUser user, string password, List<ApplicationRole> roles)
	{


		var result = await _userManager.CreateAsync(user, password);
		Console.WriteLine(result.ToString());

		if (result.Succeeded)
		{

			var savedUser = await _userManager.FindByEmailAsync(user.Email);

			var addRoleResult = await _userManager.AddToRolesAsync(user, roles.Select(r => r.Name));

			if (!addRoleResult.Succeeded) throw new InvalidOperationException("User role assignment has failed");


			return user;
		}
		else
		{
			Console.WriteLine(result.ToString());
			throw new Exception(result.ToString());
		}
	}
	public async Task<int> GetCommuterCount(){
		var usersInRole = await _userManager.GetUsersInRoleAsync("Commuter");
		return usersInRole.Count();
	}

	public async Task<double> GetLastWeekPercentageChange(){
		var usersInRole = await _userManager.GetUsersInRoleAsync("Commuter");
		var totalCount = usersInRole.Count();
		var beforeLastWeekCount = usersInRole.Count(u => u.CreatedAt.Day <= DateTime.Today.AddDays(-7).Day);
		
		if(beforeLastWeekCount == 0)
			return 0;
		return (totalCount - beforeLastWeekCount) * 100.0 / beforeLastWeekCount;
	}
	
	public async Task<Dictionary<int, int>> GetCommuterStatistics(int? year, int? month){
		var entities = await _userManager.GetUsersInRoleAsync("Commuter");
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
		}
	}
	
	public async Task<CommuterStatusDto> GetCommuterStatusCountAsync()
	{
		var commuters = await _userManager.GetUsersInRoleAsync("Commuter");
		int activeCommuters = 0;
		int idleCommuters = 0;

		if (commuters?.Count() > 0)
		{
			var thirtyDaysAgo = DateTime.Now.AddDays(-30);
			activeCommuters = commuters.Count(u => u.LastLogin >= thirtyDaysAgo);
			idleCommuters = commuters.Count(u => u.LastLogin < thirtyDaysAgo);
		}

		return new CommuterStatusDto
		{
			ActiveCommuters = activeCommuters,
			IdleCommuters = idleCommuters
		};
	}
		
}

