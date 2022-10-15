using System.Reflection;
using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<FuelType> FuelTypes => Set<FuelType>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<StationChain> StationChains => Set<StationChain>();
    public DbSet<User> Users => Set<User>();
    public DbSet<FuelStation> FuelStations => Set<FuelStation>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override int SaveChanges()
    {
        AddTimestamp();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        AddTimestamp();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void AddTimestamp()
    {
        var entities = ChangeTracker.Entries()
            .Where(e => e.Entity is ITiming && e.State is EntityState.Added or EntityState.Modified);

        foreach (EntityEntry entity in entities)
        {
            var timedEntity = (ITiming)entity.Entity;
            var timestamp = DateTime.UtcNow;

            if (entity.State == EntityState.Added)
            {
                timedEntity.CreatedAt = timestamp;
            }

            timedEntity.UpdatedAt = timestamp;
        }
    }
}