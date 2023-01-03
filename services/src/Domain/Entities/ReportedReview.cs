using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Enums;

namespace Domain.Entities;

public class ReportedReview : ICreatable, IUpdatable
{
    public long? ReviewId { get; set; }
    public Review? Review { get; set; }
    
    public long? UserId { get; set; }
    public User? User { get; set; }
    
    public ReportStatus ReportStatus { get; set; }

    public long? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public long? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}