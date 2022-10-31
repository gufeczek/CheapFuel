using System.Linq;
using Infrastructure.Persistence;

namespace WebAPI.IntegrationTests.PredefinedData;

public class FuelTypeQueryControllerData : IPredefinedData
{
    public const int InitialFuelTypesCount = 2;
    
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
            Id = 100,
            Name = "ON"
        },
        new Domain.Entities.FuelType
        {
            Id = 101,
            Name = "95"
        }
    };
}