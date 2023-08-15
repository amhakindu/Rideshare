using Npgsql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rideshare.Persistence.Repositories;
using Rideshare.Persistence.Repositories.User;
using Microsoft.Extensions.DependencyInjection;
using Rideshare.Application.Contracts.Identity;
using Rideshare.Application.Contracts.Persistence;

namespace Rideshare.Persistence;

public static class PersistenceServiceRegistration
{
    public static IServiceCollection ConfigurePersistenceService(this IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("RideshareConnectionString");
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.UseNetTopologySuite();
        var dataSourceBuild = dataSourceBuilder.Build();
        services.AddDbContext<RideshareDbContext>(opt => {
            opt.UseNpgsql(dataSourceBuild, o => o.UseNetTopologySuite()
            .EnableRetryOnFailure());
        });            

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository , UserRepository>();
        
        return services;
    }
}
