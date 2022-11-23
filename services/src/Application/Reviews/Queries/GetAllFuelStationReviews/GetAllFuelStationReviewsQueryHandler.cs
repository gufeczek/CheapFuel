using Application.Common;
using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Common.Pagination.Response;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Reviews.Queries.GetAllFuelStationReviews;

public sealed class GetAllFuelStationReviewsQueryHandler : IRequestHandler<GetAllFuelStationReviewsQuery, Page<FuelStationReviewDto>>
{
    private readonly IFuelStationRepository _fuelStationRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    public GetAllFuelStationReviewsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _fuelStationRepository = unitOfWork.FuelStations;
        _reviewRepository = unitOfWork.Reviews;
        _mapper = mapper;
    }
    
    public async Task<Page<FuelStationReviewDto>> Handle(GetAllFuelStationReviewsQuery request, CancellationToken cancellationToken)
    {
        var id = (long)request.Id!;
        if (!await _fuelStationRepository.ExistsById(id))
        {
            throw new NotFoundException($"Fuel station not found for id = {request.Id}");
        }
        
        var pageRequest = PaginationHelper.Eval(request.PageRequestDto, new FuelStationReviewDtoColumnSelector());
        var result = await _reviewRepository.GetAllForFuelStationAsync(id, pageRequest);
        return Page<FuelStationReviewDto>.From(result, _mapper.Map<IEnumerable<FuelStationReviewDto>>(result.Data));
    }
}