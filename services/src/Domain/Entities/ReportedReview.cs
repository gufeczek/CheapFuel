using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class ReportedReview : AuditableKeylessEntity
{
    public long? ReviewId { get; set; }
    public Review? Review { get; set; }
    
    public long? UserId { get; set; }
    public User? User { get; set; }
    
    public ReportStatus ReportStatus { get; set; }
}