using CloudinaryDotNet;
using Microsoft.AspNetCore.Builder;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Contracts.Persistence;
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
        services.AddSignalR();
        services.AddScoped<IRideShareHubService, RideShareHubService>();
        
        services.AddSingleton<IMapboxService>(
            provider => new MapboxService(new HttpClient(), configuration.GetSection("API_KEYS")["Mapbox"])
        );
        
        services.AddScoped<IRideshareMatchingService>(
            provider => {
                double doubleValue = 500.0;

                var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
                var mapboxService = provider.GetRequiredService<IMapboxService>();
                var backgroundJobClient = provider.GetRequiredService<IBackgroundJobClient>();

                return new RideshareMatchingService(unitOfWork, mapboxService, backgroundJobClient, doubleValue);
                // }
            }
        );
        return services;
    }

    // public static void UseInfrastructureServices(this IApplicationBuilder app)
    // {


    // }

}
