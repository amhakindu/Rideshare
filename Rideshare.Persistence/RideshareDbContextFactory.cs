using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Rideshare.Persistence;

public class RideshareDbContextFactory : IDesignTimeDbContextFactory<RideshareDbContext>
{
    public RideshareDbContext CreateDbContext(string[] args) 
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()+"/../Rideshare.WebApi/")
                .AddJsonFile("appsettings.json")
                .Build();

        var builder = new DbContextOptionsBuilder<RideshareDbContext>();
        var connectionString = configuration.GetConnectionString("RideshareConnectionString");

        builder.UseNpgsql(connectionString,o => o.UseNetTopologySuite());

        return new RideshareDbContext(builder.Options);
    }
}