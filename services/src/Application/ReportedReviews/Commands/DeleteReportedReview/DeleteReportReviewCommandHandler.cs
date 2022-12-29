using Application.Common.Exceptions;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using MediatR;

namespace Application.ReportedReviews.Commands.DeleteReportedReview;

public class DeleteReportReviewCommandHandler : IRequestHandler<DeleteReportReviewCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IReportedReviewRepository _reportedReviewRepository;
    
    public DeleteReportReviewCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _userRepository = unitOfWork.Users;
        _reviewRepository = unitOfWork.Reviews;
        _reportedReviewRepository = unitOfWork.ReportedReviews;
    }
    
    public async Task<Unit> Handle(DeleteReportReviewCommand request, CancellationToken cancellationToken)
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
       
        _reportedReviewRepository.Remove(reportedReview);
        await _unitOfWork.SaveAsync();

        return Unit.Value;
    }
}