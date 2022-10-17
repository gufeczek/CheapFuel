using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class FuelPriceRepository : BaseRepository<FuelPrice>, IFuelPriceRepository
{
    public FuelPriceRepository(AppDbContext context) : base(context) { }
}