using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rideshare.Application.Contracts.Services;
using Rideshare.Application.Features.Userss;
using Rideshare.Infrastructure.Security;
using Rideshare.Infrastructure.Services;

namespace Rideshare.Infrastructure;

public  static class InfrastructureServiceRegistration
{
     public static IServiceCollection ConfigureInfrastructureService(this IServiceCollection services, IConfiguration configuration)
    {
            
        Account account = new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]
        );
        services.AddSingleton(new Cloudinary(account));
        services.AddScoped<IResourceManager, ResourceManager>();
        services.AddScoped<IUserAccessor, UserAccessor>(); 
        return services;
    }

}
