using Application.Models;
using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelTypes.Queries.GetAllFuelTypes;

public sealed class GetAllFuelTypesQueryHandler : IRequestHandler<GetAllFuelTypesQuery, IEnumerable<FuelTypeDto>>
{
    private readonly IFuelTypeRepository _fuelTypeRepository;
    private readonly IMapper _mapper;

    public GetAllFuelTypesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _fuelTypeRepository = unitOfWork.FuelTypes;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<FuelTypeDto>> Handle(GetAllFuelTypesQuery request, CancellationToken cancellationToken)
    {
        var fuelTypes = await _fuelTypeRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<FuelTypeDto>>(fuelTypes);
    }
}