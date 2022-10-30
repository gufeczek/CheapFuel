using System.Linq;
using Infrastructure.Persistence;

namespace WebAPI.IntegrationTests.PredefinedData;

public class FuelStationServiceQueryControllerData : IPredefinedData
{
    public const int InitialFuelStationServiceCount = 2;

    public void Seed(AppDbContext dbContext)
    {
        dbContext.Services.AddRange(GetFuelTypes());
        dbContext.SaveChanges();
    }

    public void Clear(AppDbContext dbContext)
    {
        dbContext.Services.RemoveRange(dbContext.Services.ToList());
        dbContext.SaveChanges();
    }

    private static Domain.Entities.FuelStationService[] GetFuelTypes() => new[]
    {
        new Domain.Entities.FuelStationService
        {
            Id = 100,
            Name = "Toilets"
        },
        new Domain.Entities.FuelStationService
        {
            Id = 101,
            Name = "Hot dogs"
        }
    };
}