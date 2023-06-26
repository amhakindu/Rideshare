using Serilog;
using MediatR;
using Serilog.Events;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Hangfire;
using Hangfire.PostgreSql;

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
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        var connectionstring = configuration.GetSection("ConnectionStrings")["HangFireConnectionString"];
        // Add Hangfire services.
        services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(connectionstring)
        );

        // Add the processing server as IHostedService
        services.AddHangfireServer();
        return services;
    }
}
