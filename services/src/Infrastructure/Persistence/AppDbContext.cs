using System.Reflection;
using Domain.Entities;
using Domain.Entities.Tokens;
using Infrastructure.Persistence.Pipeline;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class AppDbContext : DbContext
{
    private readonly BeforeSaveChangesPipeline _beforeSaveChangesPipeline;
    
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
    public DbSet<EmailVerificationToken> EmailVerificationTokens => Set<EmailVerificationToken>();
    public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();

    public AppDbContext(DbContextOptions<AppDbContext> options, IBeforeSaveChangesPipelineBuilder builder) 
        : base(options)
    {
        _beforeSaveChangesPipeline = builder.Build();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override int SaveChanges()
    {
        _beforeSaveChangesPipeline.Invoke(ChangeTracker.Entries());
        
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        _beforeSaveChangesPipeline.Invoke(ChangeTracker.Entries());

        return base.SaveChangesAsync(cancellationToken);
    }
}