using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Models;

public sealed class CreateFuelStationReportedReviewDto
{
   public long ReviewId { get; set; }
   public long UserId { get; set; }
   public ReportStatus ReportStatus { get; set; }
}

public sealed class CreateFuelStationReportedReviewDtoProfile : Profile
{
   public CreateFuelStationReportedReviewDtoProfile()
   {
      CreateMap<ReportedReview, CreateFuelStationReportedReviewDto>();
   }
}