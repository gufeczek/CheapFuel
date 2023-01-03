using Application.Common.Authentication;
using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.ReportedReviews.Commands.CreateReportReview;

public sealed class CreateReportReviewCommandHandler : IRequestHandler<CreateReportReviewCommand, CreateFuelStationReportedReviewDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IReportedReviewRepository _reportedReviewRepository;
    private readonly IUserPrincipalService _userPrincipalService;
    private readonly IMapper _mapper;

    public CreateReportReviewCommandHandler(IUnitOfWork unitOfWork, IUserPrincipalService userPrincipalService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;
        _reportedReviewRepository = unitOfWork.ReportedReviews;
        _userPrincipalService = userPrincipalService;
        _mapper = mapper;
    }
    
    public async Task<CreateFuelStationReportedReviewDto> Handle(CreateReportReviewCommand request, CancellationToken cancellationToken)
    {
        var userId = _userPrincipalService.GetUserPrincipalId()
                     ?? throw new UnauthorizedException("User is not logged in!");
        var user = await _userRepository.GetAsync(userId)
                    ?? throw new NotFoundException($"User not found for id = {userId}");
        if (await _reportedReviewRepository.ExistsByReviewAndUserId((long)request.ReviewId!, userId))
        {
            throw new ConflictException($"User with username = {user.Username} has already report a review with id = {request.ReviewId}");
        }

        var reportedReview = new ReportedReview
        {
            ReviewId = request.ReviewId,
            UserId = userId,
            ReportStatus = ReportStatus.New
        };
        _reportedReviewRepository.Add(reportedReview);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<CreateFuelStationReportedReviewDto>(reportedReview);
    }
}