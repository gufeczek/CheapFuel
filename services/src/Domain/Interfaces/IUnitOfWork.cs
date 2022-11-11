using Domain.Interfaces.Repositories;
using Domain.Interfaces.Repositories.Tokens;

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
    IEmailVerificationTokenRepository EmailVerificationTokens { get; }
    IPasswordResetTokenRepository PasswordResetTokenRepository { get; }

    Task SaveAsync();
}