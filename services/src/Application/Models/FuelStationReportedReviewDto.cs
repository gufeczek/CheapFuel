using System.Linq.Expressions;
using Application.Models.Interfaces;
using Domain.Entities;

namespace Application.Models;

public sealed class FuelStationReportedReviewDto
{
    public long Id { get; set; }
    
    public long ReviewId { get; set; }
    
    public long UserId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
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
