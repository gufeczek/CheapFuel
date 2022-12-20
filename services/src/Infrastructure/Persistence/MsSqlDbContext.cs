using Infrastructure.Exceptions;
using Infrastructure.Persistence.Pipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence;

public sealed class MsSqlDbContext : AppDbContext
{
    private readonly IConfiguration _configuration;

    public MsSqlDbContext(
        IConfiguration configuration,
        IBeforeSaveChangesPipelineBuilder builder) : base(builder)
    {
        _configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = _configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")
                               ?? throw new AppConfigurationException("Database connection string is missing");
        options.UseSqlServer(connectionString, builder =>
        {
            builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
        });
    }
}