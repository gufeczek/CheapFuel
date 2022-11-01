using Domain.Common;

namespace Domain.Entities;

public class EmailVerificationToken : BaseEntity
{
    public string? Token { get; set; }
    public bool Expired { get; set; }
    
    public long UserId { get; set; }
    public User? User { get; set; }
}