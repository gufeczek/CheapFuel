using System.Text;
using Application.Common.Authentication;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Infrastructure.Exceptions;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
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
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        services.AddScoped<IFuelAtStationRepository, FuelAtStationRepository>();
        services.AddScoped<IFuelPriceRepository, FuelPriceRepository>();
        services.AddScoped<IFuelStationRepository, FuelStationRepository>();
        services.AddScoped<IFuelTypeRepository, FuelTypeRepository>();
        services.AddScoped<IOpeningClosingTimeRepository, OpeningClosingTimeRepository>();
        services.AddScoped<IOwnedStationRepository, OwnedStationRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddScoped<IFuelStationServiceRepository, FuelStationServiceRepository>();
        services.AddScoped<IServiceAtStationRepository, ServiceAtStationRepository>();
        services.AddScoped<IStationChainRepository, StationChainRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IUserPasswordHasher, UserPasswordHasher>();
        services.AddScoped<ITokenService, TokenService>();
        
        var authenticationSettings = new AuthenticationSettings();
        services.AddSingleton(authenticationSettings);
        
        configuration.GetSection("Authentication").Bind(authenticationSettings);
        ValidateAuthenticationSettings(authenticationSettings);
        
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = !environment.IsDevelopment();
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = authenticationSettings.Issuer,
                ValidAudience = authenticationSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.Secret))
            };
        });
    }

    private static void ValidateAuthenticationSettings(AuthenticationSettings settings)
    {
        if (settings.Secret is null 
            || settings.ExpireDays is null 
            || settings.Issuer is null 
            || settings.Audience is null)
        {
            throw new AppConfigurationException("One or more of the required authentication settings is missing");
        }
    }
}