using Application.Common.Exceptions;
using Application.Models;
using AutoMapper;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using FluentValidation;
using MediatR;

namespace Application.ReportedReviews.Commands.UpdateReportedReview;

public sealed class UpdateReportedReviewCommandHandler : IRequestHandler<UpdateReportedReviewCommand, UpdateReportedReviewDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReportedReviewRepository _reportedReviewRepository;
    private readonly IMapper _mapper;
    
    public UpdateReportedReviewCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _reportedReviewRepository = unitOfWork.ReportedReviews;
        _mapper = mapper;
    }

    public async Task<UpdateReportedReviewDto> Handle(UpdateReportedReviewCommand request, CancellationToken cancellationToken)
    {
        var reportedReview = await _reportedReviewRepository.GetByReviewId(request.ReportedReviewId!.Value)
                             ?? throw new NotFoundException($"Not found reported review with id = {request.ReportedReviewId}");

        reportedReview.ReportStatus = request.ReportedReviewDto!.ReportStatus;

        await _unitOfWork.SaveAsync();

        return _mapper.Map<UpdateReportedReviewDto>(reportedReview);
    }
}