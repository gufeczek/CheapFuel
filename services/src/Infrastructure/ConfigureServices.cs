using Infrastructure.Exceptions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddDbContext<AppDbContext>(c =>
                c.UseMySQL(configuration.GetConnectionString("DatabaseConnection") 
                           ?? throw new AppConfigurationException("Database connection string is missing")));
        }

        return services;
    }
}