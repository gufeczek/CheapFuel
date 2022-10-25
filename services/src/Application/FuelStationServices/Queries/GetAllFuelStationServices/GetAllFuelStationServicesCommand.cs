using Application.Models;
using MediatR;

namespace Application.FuelStationServices.Queries.GetAllFuelStationServices;

public class GetAllFuelStationServicesCommand : IRequest<IEnumerable<FuelStationServiceDto>>
{
    
}