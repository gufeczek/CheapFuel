namespace Domain.Common;

public abstract class BaseTimedEntity : BaseEntity, ITiming
{
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}