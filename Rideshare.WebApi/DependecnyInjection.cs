using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Domain.Models;
using Rideshare.Persistence;
using Rideshare.Persistence.Repositories.User;

namespace Rideshare.WebApi;

public static class DependencyInjection
{


    public static void AddHttpLogging(this IServiceCollection services)
    {
        services.AddHttpLogging(opt => { opt.LoggingFields = HttpLoggingFields.All; });
    }

    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecurityKey"]);
        services.AddIdentity<ApplicationUser, ApplicationRole>()
     .AddEntityFrameworkStores<RideshareDbContext>();
        services.AddAuthentication(
                 opt =>
                 {
                     opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                     opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                     opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                 }).AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         RequireExpirationTime = true,
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         RoleClaimType = "Roles",
                         ValidateIssuerSigningKey = true,
                         ValidIssuer = configuration["JwtSettings:Issuer"],
                         ValidAudience = configuration["JwtSettings:Audience"],
                         IssuerSigningKey =
                             new SymmetricSecurityKey(
                                 Encoding.UTF8.GetBytes(configuration["JwtSettings:SecurityKey"] ?? ""))
                     };
                 }
             );
        services.AddAuthorization();
        services.AddScoped<IJwtService, JwtService>();






    }
}