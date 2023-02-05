using Application.Common;
using Application.Models;
using AutoMapper;
using Domain.Common.Pagination.Response;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.FuelTypes.Queries.GetAllFuelTypes;

public sealed class GetAllFuelTypesQueryHandler : IRequestHandler<GetAllFuelTypesQuery, Page<FuelTypeDto>>
{
    private readonly IFuelTypeRepository _fuelTypeRepository;
    private readonly IMapper _mapper;

    public GetAllFuelTypesQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _fuelTypeRepository = unitOfWork.FuelTypes;
        _mapper = mapper;
    }
    
    public async Task<Page<FuelTypeDto>> Handle(GetAllFuelTypesQuery request, CancellationToken cancellationToken)
    {
        var pageRequest = PaginationHelper.Eval(request.PageRequestDto, new FuelTypeDtoColumnSelector());
        var result = await _fuelTypeRepository.GetAllAsync(pageRequest);
        return Page<FuelTypeDto>.From(result, _mapper.Map<IEnumerable<FuelTypeDto>>(result.Data));
    }
}