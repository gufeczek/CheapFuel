using Domain.Common.Interfaces;

namespace Domain.Common;

public abstract class BaseEntity : ICreatable
{
    public long Id { get; set; }
    
    public long? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
}