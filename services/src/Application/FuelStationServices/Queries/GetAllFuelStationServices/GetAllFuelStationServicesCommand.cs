using Domain.Entities;
using MediatR;

namespace Application.FuelStationServices.Queries.GetAllFuelStationServices;

public class GetAllFuelStationServicesCommand : IRequest<IEnumerable<FuelStationService>>
{
    
}