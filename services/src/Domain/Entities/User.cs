using Domain.Common;
using Domain.Enums;

namespace Domain.Entities;

public class User : PermanentEntity
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public bool? EmailConfirmed { get; set; }
    public string? Password { get; set; }
    public bool? MultiFactorAuthEnabled { get; set; }
    public Role Role { get; set; }
    public AccountStatus Status { get; set; }
}