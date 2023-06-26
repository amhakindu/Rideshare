using CloudinaryDotNet;
using Microsoft.AspNetCore.Builder;
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
        services.AddSignalR();
        services.AddScoped<IRideShareHubService, RideShareHubService>();
        return services;
    }

    public static void UseInfrastructureServices(this IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<RideShareHub>("/rideshare");
        });

    }

}
