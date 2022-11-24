using System.Linq;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;

namespace WebAPI.IntegrationTests.PredefinedData;

public class FavouriteQueryControllerData : IPredefinedData
{
    private const int StationChainId = 100;
    
    public const int FuelStation1Id = 100;
    public const int InvalidFuelStationId = 999;

    public const int UserWithFavouriteId = 200;
    public const int UserWithoutFavouriteId = 201;
    
    public const string UserWithFavouriteUsername = "User1";
    public const string UserWithoutFavouriteUsername = "User2";

    public const int InitialFavouriteCount = 1;
    
    public void Seed(AppDbContext dbContext)
    {
        dbContext.Users.AddRange(Users());
        dbContext.StationChains.AddRange(StationChains());
        dbContext.FuelStations.AddRange(FuelStations());
        dbContext.Favorites.AddRange(Favourites());
        dbContext.SaveChanges();
        
    }

    public void Clear(AppDbContext dbContext)
    {
        dbContext.Favorites.RemoveRange(dbContext.Favorites.ToList());
        dbContext.FuelStations.RemoveRange(dbContext.FuelStations.ToList());
        dbContext.StationChains.RemoveRange(dbContext.StationChains.ToList());
        dbContext.Users.RemoveRange(dbContext.Users.ToList());
        dbContext.SaveChanges();
    }
    
    private static User[] Users() => new[]
    {
        new User
        {
            Id = UserWithFavouriteId,
            Username = UserWithFavouriteUsername,
            Email = "user1@example.com",
            Password = AccountsData.DefaultPasswordHash,
            Role = Role.User,
            Status = AccountStatus.Active,
            EmailConfirmed = true,
            MultiFactorAuthEnabled = false
        },
        new User
        {
            Id = UserWithoutFavouriteId,
            Username = UserWithoutFavouriteUsername,
            Email = "user2@example.com",
            Password = AccountsData.DefaultPasswordHash,
            Role = Role.User,
            Status = AccountStatus.Active,
            EmailConfirmed = true,
            MultiFactorAuthEnabled = false
        }
    };
    
    private static Domain.Entities.StationChain[] StationChains() => new[]
    {
        new Domain.Entities.StationChain
        {
            Id = StationChainId,
            Name = "Orlen"
        }
    };

    private static Domain.Entities.FuelStation[] FuelStations() => new[]
    {
        new Domain.Entities.FuelStation
        {
            Id = FuelStation1Id,
            Name = "Fuel station 1",
            Address = new Address
            {
                City = "Lublin",
                PostalCode = "20388",
                Street = "Bursztynowa",
                StreetNumber = "1"
            },
            GeographicalCoordinates = new GeographicalCoordinates
            {
                Latitude = 50.03658019988679M,
                Longitude = 20.073312547938244M
            },
            StationChainId = StationChainId
        }
    };

    private static Favorite[] Favourites() => new[]
    {
        new Favorite
        {
            FuelStationId = FuelStation1Id,
            UserId = UserWithFavouriteId
        }
    };
}