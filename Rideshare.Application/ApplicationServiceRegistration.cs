using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Rideshare.Application;
public static class ApplicationServicesRegistration
{
    public static IServiceCollection ConfigureApplicationService(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        return services;
    }
}
