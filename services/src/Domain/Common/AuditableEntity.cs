using Domain.Common.Interfaces;

namespace Domain.Common;

public abstract class AuditableEntity : BaseEntity, IUpdatable
{
    public long? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}