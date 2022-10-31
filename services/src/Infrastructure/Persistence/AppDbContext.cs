using System.Reflection;
using Domain.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class AppDbContext : DbContext
{
    private readonly IUserPrincipalService _userPrincipalService;
    
    public DbSet<FuelType> FuelTypes => Set<FuelType>();
    public DbSet<FuelStationService> Services => Set<FuelStationService>();
    public DbSet<StationChain> StationChains => Set<StationChain>();
    public DbSet<User> Users => Set<User>();
    public DbSet<FuelStation> FuelStations => Set<FuelStation>();
    public DbSet<Favorite> Favorites => Set<Favorite>();
    public DbSet<FuelAtStation> FuelAtStations => Set<FuelAtStation>();
    public DbSet<ServiceAtStation> ServiceAtStations => Set<ServiceAtStation>();
    public DbSet<OwnedStation> OwnedStations => Set<OwnedStation>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<FuelPrice> FuelPrices => Set<FuelPrice>();

    public AppDbContext(DbContextOptions<AppDbContext> options, IUserPrincipalService userPrincipalService) 
        : base(options)
    {
        _userPrincipalService = userPrincipalService;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override int SaveChanges()
    {
        BeforeSaveChanges();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        BeforeSaveChanges();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void BeforeSaveChanges()
    {
        HandleCreation();
        HandleUpdate();
        HandleRemoval();
    }

    private void HandleCreation()
    {
        ChangeTracker.Entries()
            .Where(e => e.Entity is ICreatable && e.State is EntityState.Added)
            .Select(e => e.Entity)
            .Cast<ICreatable>()
            .ToList()
            .ForEach(e =>
            {
                e.CreatedAt = DateTime.UtcNow;
                e.CreatedBy = _userPrincipalService.GetUserPrincipalId();
            });
    }

    private void HandleUpdate()
    {
        ChangeTracker.Entries()
            .Where(e => e.Entity is IUpdatable && e.State is EntityState.Added or EntityState.Modified)
            .Select(e => e.Entity)
            .Cast<IUpdatable>()
            .ToList()
            .ForEach(e =>
            {
                e.UpdatedAt = DateTime.UtcNow;
                e.UpdatedBy =_userPrincipalService.GetUserPrincipalId();
            });
    }

    private void HandleRemoval()
    {
        ChangeTracker.Entries()
            .Where(e => e.Entity is ISoftlyDeletable && e.State is EntityState.Deleted)
            .ToList()
            .ForEach(e =>
            {
                var permanentEntity = (ISoftlyDeletable)e.Entity;

                permanentEntity.Deleted = true;
                permanentEntity.DeletedAt = DateTime.UtcNow;
                permanentEntity.DeletedBy = _userPrincipalService.GetUserPrincipalId();
            
                e.State = EntityState.Modified;
            });
    }
}