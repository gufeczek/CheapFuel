using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using WebAPI.Common.Authorization.Swagger;
using WebAPI.Common.Json;
using WebAPI.Middlewares;

namespace WebAPI;

public static class ConfigureServices
{
    public static void AddWebApiServices(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
            });
        services.AddEndpointsApiExplorer();
        
        services.AddScoped<ExceptionHandlerMiddleware>();
    }
    
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations();
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Cheap Fuel API", Version = "v1" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                In = ParameterLocation.Header,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme."
            });
            options.OperationFilter<AuthorizationOperationFilter>();
        });
    }
}