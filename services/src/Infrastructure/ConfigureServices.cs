using System.Text;
using Application.Common.Authentication;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Tokens;
using Infrastructure.Common.Services.Email;
using Infrastructure.Exceptions;
using Infrastructure.Identity;
using Infrastructure.Identity.Policies.EmailVerifiedRequirement;
using Infrastructure.Identity.Policies.UserNotBannedRequirement;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Pipeline;
using Infrastructure.Persistence.Pipeline.Operations;
using Infrastructure.Persistence.Pipeline.Operations.Interfaces;
using Infrastructure.Repositories;
using Infrastructure.Repositories.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure;

public static class ConfigureServices
{
    private const string InMemoryProvider = "InMemory";
    private const string SqlServerProvider = "SqlServer";
    private const string MySqlProvider = "MySql";
    
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var provider = configuration.GetValue<string>("DbProvider");

        switch (provider)
        {
            case InMemoryProvider:
                services.AddDbContext<AppDbContext, InMemoryDbContext>();
                break;
            case SqlServerProvider:
                services.AddDbContext<AppDbContext, MsSqlDbContext>();
                break;
            case MySqlProvider:
                services.AddDbContext<AppDbContext, MySqlDbContext>();
                break;
            default:
                throw new AppConfigurationException("Invalid database provider!");
        }
    }

    public static void AddBeforeSaveChangesPipeline(this IServiceCollection services)
    {
        services.AddScoped<IAddCreationInfoOperation, AddCreationInfoOperation>();
        services.AddScoped<IAddUpdateInfoOperation, AddUpdateInfoOperation>();
        services.AddScoped<IRemovalHandlingOperation, RemovalHandlingOperation>();
        services.AddScoped<IBeforeSaveChangesPipelineBuilder, BeforeSaveChangesPipelineBuilder>();
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
        services.AddScoped<IEmailVerificationTokenRepository, EmailVerificationTokenRepository>();
        services.AddScoped<IPasswordResetTokenRepository, PasswordResetTokenRepository>();
        services.AddScoped<IReportedReviewRepository, ReportedReviewRepository>();
        services.AddScoped<IBlockUserRepository, BlockUserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddHttpContextAccessor();
        
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IUserPasswordHasher, UserPasswordHasher>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserPrincipalService, UserPrincipalService>();

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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.Secret!))
            };
        });
    }

    public static void AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AccountActive", builder =>
            {
                builder.AddRequirements(new EmailVerifiedRequirement());
                builder.AddRequirements(new UserNotBannedRequirement());
            });
        });
        
        services.AddScoped<IAuthorizationHandler, EmailVerifiedRequirementHandler>();
        services.AddScoped<IAuthorizationHandler, UserNotBannedRequirementHandler>();
    }

    public static void AddSmtpService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEmailSenderService, EmailSenderService>();

        var emailSettings = new EmailHostSettings();
        services.AddSingleton(emailSettings);
        
        configuration.GetSection("Email").Bind(emailSettings);
        ValidateSmtpSettings(emailSettings);
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

    private static void ValidateSmtpSettings(EmailHostSettings settings)
    {
        if (settings.EmailAddress is null 
            || settings.Password is null 
            || settings.Host is null 
            || settings.Port is null)
        {
            throw new AppConfigurationException("One or more of the required email settings is missing");
        }
    }
}