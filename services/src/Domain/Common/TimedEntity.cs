namespace Domain.Common;

public abstract class TimedEntity : BaseEntity, ITiming
{
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}