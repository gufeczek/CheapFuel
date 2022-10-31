using System.Linq;
using Infrastructure.Persistence;

namespace WebAPI.IntegrationTests.PredefinedData;

public class FuelStationServiceCommandControllerData : IPredefinedData
{
    public const int InitialFuelStationServiceCount = 2;
    public const int ServiceId1 = 100;
    public const int InvalidId = 999;
    
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
            Id = ServiceId1,
            Name = "Toilets"
        },
        new Domain.Entities.FuelStationService
        {
            Id = 101,
            Name = "Hot dogs"
        }
    };
}