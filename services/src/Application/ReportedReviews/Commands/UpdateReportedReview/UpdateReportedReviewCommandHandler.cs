using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.ReportedReviews.Commands.UpdateReportedReview;

public sealed class UpdateReportedReviewCommandHandler : IRequestHandler<UpdateReportedReviewCommand, UpdateReportedReviewDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IReportedReviewRepository _reportedReviewRepository;
    private readonly IMapper _mapper;
    
    public UpdateReportedReviewCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;
        _reviewRepository = unitOfWork.Reviews;
        _reportedReviewRepository = unitOfWork.ReportedReviews;
        _mapper = mapper;
    }

    public async Task<UpdateReportedReviewDto> Handle(UpdateReportedReviewCommand request, CancellationToken cancellationToken)
    {
        if(!await _userRepository.ExistsById(request.UserId!.Value))
        {
            throw new NotFoundException($"User not found for id = {request.UserId}");
        }
        
        if(!await _reviewRepository.ExistsById(request.ReviewId!.Value))
        {
            throw new NotFoundException($"Review not found for id = {request.ReviewId}");
        }

        var reportedReview = await _reportedReviewRepository.GetByReviewIdAndUserId(request.ReviewId!.Value, request.UserId!.Value)
                             ?? throw new NotFoundException($"Not found reported review with id = {request.ReviewId}");
        reportedReview.ReportStatus = request.ReportedReviewDto!.ReportStatus;

        await _unitOfWork.SaveAsync();

        return _mapper.Map<UpdateReportedReviewDto>(reportedReview);
    }
}