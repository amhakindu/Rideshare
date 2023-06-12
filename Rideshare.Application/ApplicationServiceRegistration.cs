using Serilog;
using MediatR;
using Serilog.Events;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Rideshare.Application;
public static class ApplicationServicesRegistration
{
    public static IServiceCollection ConfigureApplicationService(this IServiceCollection services)
    {
        // Configure Serilog logging
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .WriteTo.File("Log/RideshareErrorLog.txt", rollingInterval: RollingInterval.Day)
            .WriteTo.Console()
            .CreateLogger();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        return services;
    }
}
