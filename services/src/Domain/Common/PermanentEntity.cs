using Domain.Common.Interfaces;

namespace Domain.Common;

public abstract class PermanentEntity : BaseEntity, IPermanent
{
    public long UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
    public long CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Deleted { get; set; }
    public long? DeletedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
}