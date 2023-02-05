using System;
using System.Linq;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;

namespace WebAPI.IntegrationTests.PredefinedData;

public class FuelStationQueryControllerData : IPredefinedData
{
    public const int FuelType1Id = 100;
    public const int FuelType2Id = 101;

    public const int StationChain1Id = 100;
    public const int StationChain2Id = 101;
    public const int StationChain3Id = 102;

    public const string StationChain1Name = "Orlen";
    public const string StationChain2Name = "Lotos";
    
    public const int Service1Id = 100;
    public const int Service2Id = 101;
    public const int Service3Id = 103;

    public const int FuelStation1Id = 100;
    public const int FuelStation2Id = 101;
    public const int FuelStation3Id = 102;
    public const int FuelStation4Id = 103;
    public const int FuelStation5Id = 104;

    public const int FuelStationsWithPriceOfFuelType1Count = 2;

    public const int InvalidFuelStationId = 999;
    public const int InvalidFuelTypeId = 999;
    public const int InvalidStationChainId = 999;
    public const int InvalidServiceId = 999;

    public void Seed(AppDbContext dbContext)
    {
        dbContext.FuelTypes.AddRange(FuelTypes());
        dbContext.StationChains.AddRange(StationChains());
        dbContext.Services.AddRange(Services());
        dbContext.FuelStations.AddRange(FuelStations());
        dbContext.ServiceAtStations.AddRange(ServicesAtStation());
        dbContext.FuelAtStations.AddRange(FuelAtStations());
        dbContext.FuelPrices.AddRange(FuelPrices());
        dbContext.SaveChanges();
    }

    public void Clear(AppDbContext dbContext)
    {
        dbContext.FuelPrices.RemoveRange(dbContext.FuelPrices.ToList());
        dbContext.FuelAtStations.RemoveRange(dbContext.FuelAtStations.ToList());
        dbContext.ServiceAtStations.RemoveRange(dbContext.ServiceAtStations.ToList());
        dbContext.FuelStations.RemoveRange(dbContext.FuelStations.ToList());
        dbContext.Services.RemoveRange(dbContext.Services.ToList());
        dbContext.StationChains.RemoveRange(dbContext.StationChains.ToList());
        dbContext.FuelTypes.RemoveRange(dbContext.FuelTypes.ToList());
        dbContext.SaveChanges();
    }

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
        }
    };

    private static Domain.Entities.StationChain[] StationChains() => new[]
    {
        new Domain.Entities.StationChain
        {
            Id = StationChain1Id,
            Name = StationChain1Name
        },
        new Domain.Entities.StationChain
        {
            Id = StationChain2Id,
            Name = StationChain2Name
        },
        new Domain.Entities.StationChain
        {
            Id = StationChain3Id,
            Name = "Circle K"
        }
    };

    private static Domain.Entities.FuelStationService[] Services() => new[]
    {
        new Domain.Entities.FuelStationService
        {
            Id = Service1Id,
            Name = "Toilets"
        },
        new Domain.Entities.FuelStationService
        {
            Id = Service2Id,
            Name = "Restaurant"
        },
        new Domain.Entities.FuelStationService
        {
            Id = Service3Id,
            Name = "Coffee"
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
            StationChainId = StationChain1Id
        },
        new Domain.Entities.FuelStation
        {
            Id = FuelStation2Id,
            Name = "Fuel station 2",
            Address = new Address
            {
                City = "Kraków", 
                PostalCode = "30158", 
                Street = "Ogrodowa", 
                StreetNumber = "10"
            },
            GeographicalCoordinates = new GeographicalCoordinates
            {
                Latitude = 50.06493950291983M, 
                Longitude = 19.956283459159934M
            },
            StationChainId = StationChain1Id
        },
        new Domain.Entities.FuelStation
        {
            Id = FuelStation3Id,
            Name = "Fuel station 3",
            Address = new Address
            {
                City = "Słupsk", 
                PostalCode = "76068", 
                Street = "Kwiatowa", 
                StreetNumber = "23"
            },
            GeographicalCoordinates = new GeographicalCoordinates
            {
                Latitude = 54.46889355549106M, 
                Longitude = 17.020740491606183M
            },
            StationChainId = StationChain2Id
        },
        new Domain.Entities.FuelStation
        {
            Id = FuelStation4Id,
            Name = "Fuel station 4",
            Address = new Address
            {
                City = "Bydgoszcz", 
                PostalCode = "85173", 
                Street = "Kwiatowa", 
                StreetNumber = "1"
            },
            GeographicalCoordinates = new GeographicalCoordinates
            {
                Latitude = 53.119553387997115M, 
                Longitude = 18.073487705059502M
            },
            StationChainId = StationChain2Id
        },
        new Domain.Entities.FuelStation
        {
            Id = FuelStation5Id,
            Name = "Fuel station 5",
            Address = new Address
            {
                City = "Warszawa", 
                PostalCode = "20095", 
                Street = "Kwiatowa", 
                StreetNumber = "12"
            },
            GeographicalCoordinates = new GeographicalCoordinates
            {
                Latitude = 52.237104860324735M, 
                Longitude = 20.953137189999374M
            },
            StationChainId = StationChain3Id
        }
    };

    private static ServiceAtStation[] ServicesAtStation() => new[]
    {
        new ServiceAtStation
        {
            FuelStationId = FuelStation1Id,
            ServiceId = Service1Id
        },
        new ServiceAtStation
        {
            FuelStationId = FuelStation1Id,
            ServiceId = Service2Id
        },
        new ServiceAtStation
        {
            FuelStationId = FuelStation2Id,
            ServiceId = Service1Id
        },
        new ServiceAtStation
        {
            FuelStationId = FuelStation3Id,
            ServiceId = Service2Id
        }
    };

    private static FuelAtStation[] FuelAtStations() => new[]
    {
        new FuelAtStation
        {
            FuelStationId = FuelStation1Id,
            FuelTypeId = FuelType1Id
        },
        new FuelAtStation
        {
            FuelStationId = FuelStation2Id,
            FuelTypeId = FuelType1Id
        },
        new FuelAtStation
        {
            FuelStationId = FuelStation2Id,
            FuelTypeId = FuelType2Id
        },
        new FuelAtStation
        {
            FuelStationId = FuelStation3Id,
            FuelTypeId = FuelType1Id
        },
        new FuelAtStation
        {
            FuelStationId = FuelStation3Id,
            FuelTypeId = FuelType2Id
        },
        new FuelAtStation
        {
            FuelStationId = FuelStation4Id,
            FuelTypeId = FuelType1Id
        },
        new FuelAtStation
        {
            FuelStationId = FuelStation4Id,
            FuelTypeId = FuelType2Id
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
            FuelStationId = FuelStation1Id,
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
            FuelStationId = FuelStation1Id,
            FuelTypeId = FuelType1Id,
            UserId = AccountsData.UserId,
            CreatedAt = new DateTime(2022, 1, 2)
        },
        new Domain.Entities.FuelPrice
        {
            Price = 2.43M,
            Available = true,
            Status = FuelPriceStatus.Accepted,
            Priority = false,
            FuelStationId = FuelStation1Id,
            FuelTypeId = FuelType1Id,
            UserId = AccountsData.UserId,
            CreatedAt = new DateTime(2022, 1, 3)
        },
        new Domain.Entities.FuelPrice
        {
            Price = 2.21M,
            Available = true,
            Status = FuelPriceStatus.Accepted,
            Priority = false,
            FuelStationId = FuelStation1Id,
            FuelTypeId = FuelType1Id,
            UserId = AccountsData.UserId,
            CreatedAt = new DateTime(2022, 1, 3)
        },
        new Domain.Entities.FuelPrice
        {
            Price = 2.14M,
            Available = true,
            Status = FuelPriceStatus.Accepted,
            Priority = false,
            FuelStationId = FuelStation1Id,
            FuelTypeId = FuelType1Id,
            UserId = AccountsData.UserId,
            CreatedAt = new DateTime(2022, 1, 4)
        },
        new Domain.Entities.FuelPrice
        {
            Price = 3.52M,
            Available = true,
            Status = FuelPriceStatus.Accepted,
            Priority = false,
            FuelStationId = FuelStation2Id,
            FuelTypeId = FuelType1Id,
            UserId = AccountsData.UserId,
            CreatedAt = new DateTime(2022, 1, 1)
        },
        new Domain.Entities.FuelPrice
        {
            Price = 3.56M,
            Available = true,
            Status = FuelPriceStatus.Accepted,
            Priority = false,
            FuelStationId = FuelStation2Id,
            FuelTypeId = FuelType1Id,
            UserId = AccountsData.UserId,
            CreatedAt = new DateTime(2022, 1, 4)
        },
        new Domain.Entities.FuelPrice
        {
            Price = 4.56M,
            Available = true,
            Status = FuelPriceStatus.Accepted,
            Priority = false,
            FuelStationId = FuelStation2Id,
            FuelTypeId = FuelType2Id,
            UserId = AccountsData.UserId,
            CreatedAt = new DateTime(2022, 1, 4)
        },
        new Domain.Entities.FuelPrice
        {
            Price = 4.26M,
            Available = true,
            Status = FuelPriceStatus.Accepted,
            Priority = false,
            FuelStationId = FuelStation3Id,
            FuelTypeId = FuelType1Id,
            UserId = AccountsData.UserId,
            CreatedAt = new DateTime(2022, 1, 4)
        },
        new Domain.Entities.FuelPrice
        {
            Price = 2.51M,
            Available = true,
            Status = FuelPriceStatus.Accepted,
            Priority = false,
            FuelStationId = FuelStation3Id,
            FuelTypeId = FuelType1Id,
            UserId = AccountsData.UserId,
            CreatedAt = new DateTime(2022, 1, 5)
        },
        new Domain.Entities.FuelPrice
        {
            Price = 2.54M,
            Available = true,
            Status = FuelPriceStatus.Accepted,
            Priority = false,
            FuelStationId = FuelStation3Id,
            FuelTypeId = FuelType2Id,
            UserId = AccountsData.UserId,
            CreatedAt = new DateTime(2022, 1, 4)
        },
        new Domain.Entities.FuelPrice
        {
            Price = 2.52M,
            Available = true,
            Status = FuelPriceStatus.Accepted,
            Priority = false,
            FuelStationId = FuelStation3Id,
            FuelTypeId = FuelType2Id,
            UserId = AccountsData.UserId,
            CreatedAt = new DateTime(2022, 1, 5)
        },
        new Domain.Entities.FuelPrice
        {
            Price = 4.54M,
            Available = true,
            Status = FuelPriceStatus.Accepted,
            Priority = false,
            FuelStationId = FuelStation4Id,
            FuelTypeId = FuelType1Id,
            UserId = AccountsData.UserId,
            CreatedAt = new DateTime(2022, 1, 4)
        },
        new Domain.Entities.FuelPrice
        {
            Price = 4.54M,
            Available = true,
            Status = FuelPriceStatus.Accepted,
            Priority = false,
            FuelStationId = FuelStation5Id,
            FuelTypeId = FuelType1Id,
            UserId = AccountsData.UserId,
            CreatedAt = new DateTime(2022, 1, 4)
        }
    };
}