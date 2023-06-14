using Rideshare.Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rideshare.Persistence.Repositories;

namespace Rideshare.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection ConfigurePersistenceService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RideshareDbContext>(opt =>
        opt.UseNpgsql(configuration.GetConnectionString("RideshareConnectionString"),o => o.UseNetTopologySuite()
        .EnableRetryOnFailure()));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        

        return services;
    }
}
