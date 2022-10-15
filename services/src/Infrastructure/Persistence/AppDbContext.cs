using System.Reflection;
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Infrastructure.Persistence;

public sealed class AppDbContext : DbContext
{
    public DbSet<FuelType> FuelTypes => Set<FuelType>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<StationChain> StationChains => Set<StationChain>();
    public DbSet<User> Users => Set<User>();
    public DbSet<FuelStation> FuelStations => Set<FuelStation>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        ChangeTracker.CascadeDeleteTiming = CascadeTiming.OnSaveChanges;
        ChangeTracker.DeleteOrphansTiming = CascadeTiming.OnSaveChanges;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override int SaveChanges()
    {
        AddTimestamp();
        AddPrincipal();
        HandleRemoval();
        
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        AddTimestamp();
        AddPrincipal();
        HandleRemoval();

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

    private void AddPrincipal()
    {
        var entities = ChangeTracker.Entries()
            .Where(e => e.Entity is ITracked && e.State is EntityState.Added or EntityState.Modified);

        foreach (EntityEntry entity in entities)
        {
            var trackedEntity = (ITracked)entity.Entity;
            var principal = 1L; // Just for now, should be change after Issue #4

            if (entity.State == EntityState.Added)
            {
                trackedEntity.CreatedBy = principal;
            }

            trackedEntity.UpdatedBy = principal;
        }
    }

    private void HandleRemoval()
    {
        var entities = ChangeTracker.Entries()
            .Where(e => e.Entity is IPermanent && e.State is EntityState.Deleted);

        foreach (EntityEntry entity in entities)
        {
            var permanentEntity = (IPermanent)entity.Entity;

            permanentEntity.Deleted = true;
            permanentEntity.DeletedAt = DateTime.UtcNow;
            permanentEntity.DeletedBy = 1L; // Just for now, should be change after Issue #4
            
            entity.State = EntityState.Modified;
        }
    }
}