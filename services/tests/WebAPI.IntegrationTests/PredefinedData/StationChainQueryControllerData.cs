using System.Linq;
using Infrastructure.Persistence;

namespace WebAPI.IntegrationTests.PredefinedData;

public class StationChainQueryControllerData : IPredefinedData
{
    public const int InitialStationChainsCount = 2;

    public void Seed(AppDbContext dbContext)
    {
        dbContext.StationChains.AddRange(GetStationChains());
        dbContext.SaveChanges();
    }

    public void Clear(AppDbContext dbContext)
    {
        dbContext.StationChains.RemoveRange(dbContext.StationChains.ToList());
        dbContext.SaveChanges();
    }
    
    private Domain.Entities.StationChain[] GetStationChains() => new[]
    {
        new Domain.Entities.StationChain
        {
            Id = 100,
            Name = "Lotos"
        },
        new Domain.Entities.StationChain
        {
            Id = 101,
            Name = "BP"
        }
    };
}