using System.Linq.Expressions;
using Application.Models.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Models;

public sealed class FuelStationReportedReviewDto
{
    public long ReviewId { get; set; }
    
    public long UserId { get; set; }
    
    public ReportStatus ReportStatus { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}

public sealed class FuelStationReportedReviewDtoProfile : Profile
{
    public FuelStationReportedReviewDtoProfile()
    {
        CreateMap<ReportedReview, FuelStationReportedReviewDto>();
    }
}
public sealed class FuelStationReportedReviewDtoColumnSelector : IColumnSelector<ReportedReview>
{
    public Dictionary<string, Expression<Func<ReportedReview, object>>> ColumnSelector { get; } = new()
    {
        { nameof(ReportedReview.CreatedAt), r => r.CreatedAt },
        { nameof(ReportedReview.UpdatedAt), r => r.UpdatedAt },
        { nameof(ReportedReview.ReportStatus), r => r.ReportStatus! }
    };
}
