using Domain.Interfaces.Repositories;

namespace Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IFavoriteRepository Favorites { get; }
    IFuelAtStationRepository FuelsAtStation { get; }
    IFuelPriceRepository FuelPrices { get; }
    IFuelStationRepository FuelStations { get; }
    IFuelTypeRepository FuelTypes { get; }
    IOpeningClosingTimeRepository OpeningClosingTimes { get; }
    IOwnedStationRepository OwnedStations { get; }
    IReviewRepository Reviews { get; }
    IServiceAtStationRepository ServicesAtStation { get; }
    IFuelStationServiceRepository Services { get; }
    IStationChainRepository StationChains { get; }
    IUserRepository Users { get; }

    Task SaveAsync();
}