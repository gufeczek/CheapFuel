using Domain.Common;

namespace Domain.Entities.Tokens;

public abstract class AbstractToken : BaseEntity
{
    public string? Token { get; set; }
    public int Count { get; set; }
    
    public long UserId { get; set; }
    public User? User { get; set; }
}