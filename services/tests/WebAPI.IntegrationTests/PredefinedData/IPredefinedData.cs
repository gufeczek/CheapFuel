using Infrastructure.Persistence;

namespace WebAPI.IntegrationTests.PredefinedData;

public interface IPredefinedData
{
    void Seed(AppDbContext dbContext);
    void Clear(AppDbContext dbContext);
}