using Infrastructure.Persistence.Pipeline;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class InMemoryDbContext : AppDbContext
{
    public InMemoryDbContext(IBeforeSaveChangesPipelineBuilder builder) 
        : base(builder) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseInMemoryDatabase("CheapFuelDB");
    }
}