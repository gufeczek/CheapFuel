using Infrastructure.Persistence.Pipeline;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class InMemoryDbContext : AppDbContext
{
    public InMemoryDbContext(DbContextOptions options, IBeforeSaveChangesPipelineBuilder builder) 
        : base(options, builder) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseInMemoryDatabase("CheapFuelDB");
    }
}