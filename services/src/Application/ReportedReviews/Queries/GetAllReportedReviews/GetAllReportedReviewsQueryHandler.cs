using Application.Common;
using Application.Models;
using AutoMapper;
using Domain.Common.Pagination.Response;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.ReportedReviews.Queries.GetAllReportedReviews;

public class GetAllReportedReviewsQueryHandler : IRequestHandler<GetAllReportedReviewsQuery, Page<FuelStationReportedReviewDto>>
{
    private readonly IReportedReviewRepository _reportedReviewRepository;
    private readonly IMapper _mapper;

    public GetAllReportedReviewsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _reportedReviewRepository = unitOfWork.ReportedReviews;
        _mapper = mapper;
    }
    public async Task<Page<FuelStationReportedReviewDto>> Handle(GetAllReportedReviewsQuery request, CancellationToken cancellationToken)
    {
        var pageRequest = PaginationHelper.Eval(request.PageRequestDto, new FuelStationReportedReviewDtoColumnSelector());
        var result = await _reportedReviewRepository.GetAllAsync(pageRequest);
        return Page<FuelStationReportedReviewDto>.From(result, _mapper.Map<IEnumerable<FuelStationReportedReviewDto>>(result.Data));
    }
}