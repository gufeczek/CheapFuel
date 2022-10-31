using System.Linq;
using Infrastructure.Persistence;

namespace WebAPI.IntegrationTests.PredefinedData;

public class FuelTypeCommandControllerData : IPredefinedData
{
    public const int InitialFuelTypesCount = 2;
    public const int FuelTypeId1 = 100;
    public const int InvalidId = 999;
    
    public void Seed(AppDbContext dbContext)
    {
        dbContext.FuelTypes.AddRange(GetFuelTypes());
        dbContext.SaveChanges();
    }

    public void Clear(AppDbContext dbContext)
    {
        dbContext.FuelTypes.RemoveRange(dbContext.FuelTypes.ToList());
        dbContext.SaveChanges();
    }

    private static Domain.Entities.FuelType[] GetFuelTypes() => new[]
    {
        new Domain.Entities.FuelType
        {
            Id = FuelTypeId1,
            Name = "ON"
        },
        new Domain.Entities.FuelType
        {
            Id = 101,
            Name = "95"
        }
    };
}