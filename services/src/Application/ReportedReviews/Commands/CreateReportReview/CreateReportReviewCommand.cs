using Application.Models;
using Domain.Enums;
using MediatR;

namespace Application.ReportedReviews.Commands.CreateReportReview;

public sealed record CreateReportReviewCommand(long? ReviewId) : IRequest<CreateFuelStationReportedReviewDto>;
