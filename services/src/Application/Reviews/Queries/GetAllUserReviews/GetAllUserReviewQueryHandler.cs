using Application.Common;
using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Common.Pagination.Response;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.Reviews.Queries.GetAllUserReviews;

public class GetAllUserReviewQueryHandler : IRequestHandler<GetAllUserReviewsQuery, Page<FuelStationReviewDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;

    public GetAllUserReviewQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _userRepository = unitOfWork.Users;
        _reviewRepository = unitOfWork.Reviews;
        _mapper = mapper;
    }
    
    public async Task<Page<FuelStationReviewDto>> Handle(GetAllUserReviewsQuery request, CancellationToken cancellationToken)
    {
        var username = request.Username!;
        if (!await _userRepository.ExistsByUsername(username))
        {
            throw new NotFoundException($"User not found for username = {username}");
        }

        var pageRequest = PaginationHelper.Eval(request.PageRequest, new FuelStationReviewDtoColumnSelector());
        var result = await _reviewRepository.GetAllForUserAsync(username, pageRequest);
        return Page<FuelStationReviewDto>.From(result, _mapper.Map<IEnumerable<FuelStationReviewDto>>(result.Data));
    }
}