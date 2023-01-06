using Domain.Common;

namespace Domain.Entities;

public class BlockedUser : BaseEntity
{
    public long? UserId { get; set; }
    public User? User { get; set; }
    
    public DateTime StartBanDate { get; set; }
    public DateTime EndBanDate { get; set; }
    public string? Reason { get; set; }
    
}