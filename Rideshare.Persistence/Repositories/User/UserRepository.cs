using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

using System.Security.Claims;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Domain.Models;
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
            Paginated= filteredUsers,
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
        var users = await _userManager.Users.ToListAsync();
        var filteredUsers = users.
            Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        var paginatedResponse = new PaginatedResponse<ApplicationUser>
        {
            Paginated = filteredUsers,
            Count = users.Count
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

    public async Task<int> GetCommuterCount(DateTime date)
    {
        var role = "Commuter";

        var usersInRole = await _userManager.GetUsersInRoleAsync(role);
        var count = usersInRole.Count(u => u.CreatedAt.Date == date.Date);

        return count;
    }






}

