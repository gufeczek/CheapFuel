using System;
using System.Linq;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;

namespace WebAPI.IntegrationTests.PredefinedData;

public class ReviewQueryControllerData : IPredefinedData
{
    private const int User1Id = 200;
    private const int User2Id = 201;
    private const int User3Id = 202;

    public const string User1Username = "User 1";
    public const string User2Username = "User 2";
    public const string User3Username = "User 3";

    private const int StationChain1Id = 100;

    public const int FuelStation1Id = 100;
    public const int FuelStation2Id = 101;
    public const int InvalidFuelStationId = 999;

    public const int FuelStation1InitialReviewCount = 2;

    public void Seed(AppDbContext dbContext)
    {
        dbContext.Users.AddRange(Users());
        dbContext.StationChains.AddRange(StationChains());
        dbContext.FuelStations.AddRange(FuelStations());
        dbContext.Reviews.AddRange(Reviews());
        dbContext.SaveChanges();
    }

    public void Clear(AppDbContext dbContext)
    {
        dbContext.Reviews.RemoveRange(dbContext.Reviews.ToList());
        dbContext.FuelStations.RemoveRange(dbContext.FuelStations.ToList());
        dbContext.StationChains.RemoveRange(dbContext.StationChains.ToList());
        dbContext.Users.RemoveRange(dbContext.Users.ToList());
        dbContext.SaveChanges();
    }

    private static User[] Users() => new[]
    {
        new User
        {
            Id = User1Id,
            Username = User1Username,
            Email = "user1@example.com",
            Password = AccountsData.DefaultPasswordHash,
            Role = Role.User,
            Status = AccountStatus.Active,
            EmailConfirmed = true,
            MultiFactorAuthEnabled = false
        },
        new User
        {
            Id = User2Id,
            Username = User2Username,
            Email = "user2@example.com",
            Password = AccountsData.DefaultPasswordHash,
            Role = Role.User,
            Status = AccountStatus.Active,
            EmailConfirmed = true,
            MultiFactorAuthEnabled = false
        },
        new User
        {
            Id = User3Id,
            Username = User3Username,
            Email = "user3@example.com",
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
            Id = StationChain1Id,
            Name = "Orlen"
        },
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
                City = "Lublin",
                PostalCode = "20388",
                Street = "Nowomiejska",
                StreetNumber = "14"
            },
            GeographicalCoordinates = new GeographicalCoordinates
            {
                Latitude = 50.06493950291983M,
                Longitude = 19.956283459159934M
            },
            StationChainId = StationChain1Id
        }
    };

    private static Domain.Entities.Review[] Reviews() => new[]
    {
        new Domain.Entities.Review
        {
            Id = 100,
            Rate = 5,
            Content = "Lorem ipsum",
            UserId = User1Id,
            FuelStationId = FuelStation1Id,
            CreatedAt = new DateTime(2022, 10, 1),
            CreatedBy = User1Id,
            UpdatedAt = new DateTime(2022, 10, 1),
            UpdatedBy = User1Id
        },
        new Domain.Entities.Review
        {
            Id = 101,
            Rate = 3,
            Content = null,
            UserId = User2Id,
            FuelStationId = FuelStation1Id,
            CreatedAt = new DateTime(2022, 10, 1),
            CreatedBy = User2Id,
            UpdatedAt = new DateTime(2022, 10, 1),
            UpdatedBy = User2Id
        },
    };
}