using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rideshare.Application.Contracts.Services;

namespace Rideshare.Infrastructure;

public  static class InfrastructureServiceRegistration
{
     public static IServiceCollection ConfigureInfrastructureService(this IServiceCollection services, IConfiguration configuration)
    {
     return services;
    }

}
