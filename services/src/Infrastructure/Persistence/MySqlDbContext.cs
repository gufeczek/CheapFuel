using Infrastructure.Exceptions;
using Infrastructure.Persistence.Pipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence;

public sealed class MySqlDbContext : AppDbContext
{
    private readonly IConfiguration _configuration;

    public MySqlDbContext(
        IConfiguration configuration,
        IBeforeSaveChangesPipelineBuilder builder) : base(builder)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = _configuration.GetConnectionString("DatabaseConnection")
                                  ?? throw new AppConfigurationException("Database connection string is missing");
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 0));

        options.UseMySql(connectionString, serverVersion)
            .LogTo(Console.WriteLine, LogLevel.Information)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors();
    }
}