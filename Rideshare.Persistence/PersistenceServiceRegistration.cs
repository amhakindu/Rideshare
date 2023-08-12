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
        services.AddDbContext<RideshareDbContext>(opt => {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            dataSourceBuilder.UseNetTopologySuite();
            var dataSource = dataSourceBuilder.Build();

            opt.UseNpgsql(dataSource, o => o.UseNetTopologySuite()
            .EnableRetryOnFailure());
        });

        // Read the Haversine function script from the file.
        string functionScript;

        // Combine the provided path segments
        string scriptFilePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).FullName, "Rideshare.Persistence", "Configurations", "Routines", "HaversineDistance.sql");
        
        if (File.Exists(scriptFilePath))
            functionScript = File.ReadAllText(scriptFilePath);
        else
        {
            throw new FileNotFoundException($"Haversine function script not found at '{scriptFilePath}'.");
        }
        
        // Check and create the Haversine function.
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            string functionCheckQuery = "SELECT COUNT(*) FROM information_schema.routines WHERE specific_schema = 'public' AND specific_name = 'haversine_distance' AND routine_type = 'FUNCTION'";

            using (var command = new NpgsqlCommand(functionCheckQuery, connection))
            {
                long functionExists = (long)command.ExecuteScalar();

                if (functionExists == 0)
                {
                    using (var createFunctionCommand = new NpgsqlCommand(functionScript, connection))
                    {
                        createFunctionCommand.ExecuteNonQuery();
                    }
                    Console.WriteLine("Successfully updated the haversine distance function!");
                }
            }
        }

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository , UserRepository>();
        
        return services;
    }
}
