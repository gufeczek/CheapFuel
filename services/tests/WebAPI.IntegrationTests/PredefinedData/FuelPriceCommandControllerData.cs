using System;
using System.Linq;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;

namespace WebAPI.IntegrationTests.PredefinedData;

public class FuelPriceCommandControllerData : IPredefinedData
{
    private const long FuelStationOwnerId = 201;
    public const string FuelStationOwnerUsername = "FuelStationOwner";
    
    public const int FuelType1Id = 100;
    public const int FuelType2Id = 101;
    public const int FuelType3Id = 102;

    private const int StationChainId = 200;

    public const int OwnedFuelStationId = 100;
    public const int NotOwnedFuelStationId = 101;
    public const int InvalidFuelStationId = 999;

    public const int FuelPriceInitialCount = 2;
    
    public void Seed(AppDbContext dbContext)
    {
        dbContext.Users.AddRange(Users());
        dbContext.FuelTypes.AddRange(FuelTypes());
        dbContext.StationChains.AddRange(StationChains());
        dbContext.FuelStations.AddRange(FuelStations());
        dbContext.FuelAtStations.AddRange(FuelAtStations());
        dbContext.FuelPrices.AddRange(FuelPrices());
        dbContext.OwnedStations.AddRange(OwnedStations());
        dbContext.SaveChanges();
    }

    public void Clear(AppDbContext dbContext)
    {
        dbContext.OwnedStations.RemoveRange(dbContext.OwnedStations.ToList());
        dbContext.FuelPrices.RemoveRange(dbContext.FuelPrices.ToList());
        dbContext.FuelAtStations.RemoveRange(dbContext.FuelAtStations.ToList());
        dbContext.FuelStations.RemoveRange(dbContext.FuelStations.ToList());
        dbContext.StationChains.RemoveRange(dbContext.StationChains.ToList());
        dbContext.FuelTypes.RemoveRange(dbContext.FuelTypes.ToList());
        dbContext.Users.RemoveRange(dbContext.Users.ToList());
        dbContext.SaveChanges();
    }

    private static User[] Users() => new[]
    {
        new User
        {
            Id = FuelStationOwnerId,
            Username = FuelStationOwnerUsername,
            Email = $"{FuelStationOwnerUsername}@gmail.com",
            Password = AccountsData.DefaultPasswordHash,
            Role = Role.Owner,
            Status = AccountStatus.Active,
            EmailConfirmed = true,
            MultiFactorAuthEnabled = false
        }
    };
    
    private static Domain.Entities.FuelType[] FuelTypes() => new[]
    {
        new Domain.Entities.FuelType
        {
            Id = FuelType1Id,
            Name = "Pb 95"
        },
        new Domain.Entities.FuelType
        {
            Id = FuelType2Id,
            Name = "Pb 98"
        },
        new Domain.Entities.FuelType
        {
            Id = FuelType3Id,
            Name = "NO"
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
            Id = OwnedFuelStationId,
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
        },
        new Domain.Entities.FuelStation
        {
            Id = NotOwnedFuelStationId,
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
        },
    };

    private static FuelAtStation[] FuelAtStations() => new[]
    {
        new FuelAtStation
        {
            FuelStationId = OwnedFuelStationId,
            FuelTypeId = FuelType1Id
        },
        new FuelAtStation
        {
            FuelStationId = OwnedFuelStationId,
            FuelTypeId = FuelType2Id
        },
    };

    private static OwnedStation[] OwnedStations() => new[]
    {
        new OwnedStation
        {
            UserId = FuelStationOwnerId,
            FuelStationId = OwnedFuelStationId
        }
    };

    private static Domain.Entities.FuelPrice[] FuelPrices() => new[]
    {
        new Domain.Entities.FuelPrice
        {
            Price = 2.23M,
            Available = true,
            Status = FuelPriceStatus.Accepted,
            Priority = false,
            FuelStationId = OwnedFuelStationId,
            FuelTypeId = FuelType1Id,
            UserId = AccountsData.UserId,
            CreatedAt = new DateTime(2022, 1, 1)
        },
        new Domain.Entities.FuelPrice
        {
            Price = 2.12M,
            Available = true,
            Status = FuelPriceStatus.Accepted,
            Priority = false,
            FuelStationId = OwnedFuelStationId,
            FuelTypeId = FuelType1Id,
            UserId = AccountsData.UserId,
            CreatedAt = new DateTime(2022, 1, 2)
        }
    };
}