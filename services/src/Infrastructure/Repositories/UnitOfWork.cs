using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Tokens;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    
    public IFavoriteRepository Favorites { get; }
    public IFuelAtStationRepository FuelsAtStation { get; }
    public IFuelPriceRepository FuelPrices { get; }
    public IFuelStationRepository FuelStations { get; }
    public IFuelTypeRepository FuelTypes { get; }
    public IOpeningClosingTimeRepository OpeningClosingTimes { get; }
    public IOwnedStationRepository OwnedStations { get; }
    public IReviewRepository Reviews { get; }
    public IServiceAtStationRepository ServicesAtStation { get; }
    public IFuelStationServiceRepository Services { get; }
    public IStationChainRepository StationChains { get; }
    public IUserRepository Users { get; }
    public IEmailVerificationTokenRepository EmailVerificationTokens { get; }
    public IPasswordResetTokenRepository PasswordResetTokenRepository { get; }
    
    public UnitOfWork(AppDbContext context, 
        IFavoriteRepository favorites, 
        IFuelAtStationRepository fuelsAtStation, 
        IFuelPriceRepository fuelPrices, 
        IFuelStationRepository fuelStations, 
        IFuelTypeRepository fuelTypes, 
        IOpeningClosingTimeRepository openingClosingTimes,
        IOwnedStationRepository ownedStations, 
        IReviewRepository reviews, 
        IServiceAtStationRepository servicesAtStation, 
        IFuelStationServiceRepository services, 
        IStationChainRepository stationChains,
        IUserRepository users,
        IEmailVerificationTokenRepository emailVerificationTokens,
        IPasswordResetTokenRepository passwordResetTokenRepository)
    {
        _context = context;
        Favorites = favorites;
        FuelsAtStation = fuelsAtStation;
        FuelPrices = fuelPrices;
        FuelStations = fuelStations;
        FuelTypes = fuelTypes;
        OpeningClosingTimes = openingClosingTimes;
        OwnedStations = ownedStations;
        Reviews = reviews;
        ServicesAtStation = servicesAtStation;
        Services = services;
        StationChains = stationChains;
        Users = users;
        EmailVerificationTokens = emailVerificationTokens;
        PasswordResetTokenRepository = passwordResetTokenRepository;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
    
    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}