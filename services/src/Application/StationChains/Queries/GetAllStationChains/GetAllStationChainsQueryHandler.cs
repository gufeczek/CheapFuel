using Application.Common;
using Application.Models;
using AutoMapper;
using Domain.Common.Pagination.Response;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.StationChains.Queries.GetAllStationChains;

public class GetAllStationChainsQueryHandler : IRequestHandler<GetAllStationChains.GetAllStationChainsQuery, Page<StationChainDto>>
{
    private readonly IStationChainRepository _stationChainRepository;
    private readonly IMapper _mapper;

    public GetAllStationChainsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _stationChainRepository = unitOfWork.StationChains;
        _mapper = mapper;
    }
    
    public async Task<Page<StationChainDto>> Handle(GetAllStationChains.GetAllStationChainsQuery request, CancellationToken cancellationToken)
    {
        var pageRequest = PaginationHelper.Eval(request.PageRequestDto, new StationChainDtoColumnSelector());
        var result = await _stationChainRepository.GetAllAsync(pageRequest);
        return Page<StationChainDto>.From(result, _mapper.Map<IEnumerable<StationChainDto>>(result.Data));
    }
}