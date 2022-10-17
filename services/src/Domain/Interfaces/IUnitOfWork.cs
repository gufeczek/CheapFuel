using Domain.Interfaces.Repositories;

namespace Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    public IFavoriteRepository Favorites { get; }
    public IFuelAtStationRepository FuelsAtStation { get; }
    public IFuelPriceRepository FuelPrices { get; }
    public IFuelStationRepository FuelStations { get; }
    public IFuelTypeRepository FuelTypes { get; }
    public IOpeningClosingTimeRepository OpeningClosingTimes { get; }
    public IOwnedStationRepository OwnedStations { get; }
    public IReviewRepository Reviews { get; }
    public IServiceAtStationRepository ServicesAtStation { get; }
    public IServiceRepository Services { get; }
    public IStationChainRepository StationChains { get; }
    public IUserRepository Users { get; }

    Task SaveAsync();
}