using Domain.Common.Interfaces;

namespace Domain.Common;

public abstract class AuditableKeylessEntity : ICreatable, IUpdatable
{
    public long? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public long? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}