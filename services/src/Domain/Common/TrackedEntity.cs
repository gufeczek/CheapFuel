using Domain.Common.Interfaces;

namespace Domain.Common;

public abstract class TrackedEntity : BaseEntity, ITracked
{
    public long UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}