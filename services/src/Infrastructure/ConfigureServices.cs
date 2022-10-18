using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure.Exceptions;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
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

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        services.AddScoped<IFuelAtStationRepository, FuelAtStationRepository>();
        services.AddScoped<IFuelPriceRepository, FuelPriceRepository>();
        services.AddScoped<IFuelStationRepository, FuelStationRepository>();
        services.AddScoped<IFuelTypeRepository, FuelTypeRepository>();
        services.AddScoped<IOpeningClosingTimeRepository, OpeningClosingTimeRepository>();
        services.AddScoped<IOwnedStationRepository, OwnedStationRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IServiceAtStationRepository, ServiceAtStationRepository>();
        services.AddScoped<IStationChainRepository, StationChainRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}