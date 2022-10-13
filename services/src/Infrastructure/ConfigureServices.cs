using Infrastructure.Exceptions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<AppDbContext>(c =>
                c.UseInMemoryDatabase("CheapFuelDB"));
        }
        else
        {
            var connectionString = configuration.GetConnectionString("DatabaseConnection")
                                   ?? throw new AppConfigurationException("Database connection string is missing");
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 0));

            services.AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString, serverVersion)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors());
        }

        return services;
    }
}