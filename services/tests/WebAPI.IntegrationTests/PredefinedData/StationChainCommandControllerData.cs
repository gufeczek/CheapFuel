using System.Linq;
using Infrastructure.Persistence;

namespace WebAPI.IntegrationTests.PredefinedData;

public class StationChainCommandControllerData : IPredefinedData
{
    public const int InitialStationChainsCount = 2;
    public const int StationChainId1 = 100;
    public const int InvalidId = 999;

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
            Id = StationChainId1,
            Name = "Lotos"
        },
        new Domain.Entities.StationChain
        {
            Id = 101,
            Name = "BP"
        }
    };
}