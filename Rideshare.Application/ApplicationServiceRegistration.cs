using Serilog;
using MediatR;
using Serilog.Events;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Rideshare.Application.Profiles;
using Rideshare.Application.Contracts.Infrastructure;
using Rideshare.Application.Contracts.Persistence;
using AutoMapper;

namespace Rideshare.Application;
public static class ApplicationServicesRegistration
{
    public static IServiceCollection ConfigureApplicationService(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure Serilog logging
        Log.Logger = new LoggerConfiguration()
            // .MinimumLevel.Debug()
            .MinimumLevel.Information()
            .WriteTo.File("Log/RideshareErrorLog.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.Console()
            .CreateLogger();

        services.AddScoped<IMapper>(
            provider => {
                var unitOfWork = provider.GetRequiredService<IUnitOfWork>();
                var mapboxService = provider.GetRequiredService<IMapboxService>();

                var profile = new MappingProfile(mapboxService, unitOfWork);
                var configuration = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(profile);
                });
                return configuration.CreateMapper();
            }
        );
        services.AddMediatR(Assembly.GetExecutingAssembly());
        return services;
    }
}
