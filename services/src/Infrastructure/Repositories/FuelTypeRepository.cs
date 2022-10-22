using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class FuelTypeRepository : BaseRepository<FuelType>, IFuelTypeRepository
{
    public FuelTypeRepository(AppDbContext context) : base(context) { }
}