using Microsoft.OpenApi.Models;
using WebAPI.Common.Authorization.Swagger;

namespace WebAPI;

public static class ConfigureServices
{
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