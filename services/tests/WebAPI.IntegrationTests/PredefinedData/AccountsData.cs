using System.Linq;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;

namespace WebAPI.IntegrationTests.PredefinedData;

public class AccountsData : IPredefinedData
{
    public const string DefaultPassword = "Password123";
    public const string DefaultPasswordHash = "AQAAAAEAACcQAAAAEIx4l5FIMC2QHbCl94VCmPBY6//9LqJfoCifq8a5vxVDbfk4CMwLB6JAL0kSDgj+kA==";

    public const int InitialUserCount = 3;
    
    public const string UserUsername = "User";
    public const string OwnerUsername = "Owner";
    public const string AdminUsername = "Admin";
    
    public const string UserEmailAddress = "user@gmail.com";

    public const string InvalidUsername = "InvalidUsername";
    public const string InvalidPassword = "InvalidPassword";
    public const string InvalidEmail = "invalid@gmail.com";
    
    public void Seed(AppDbContext dbContext)
    {
        dbContext.Users.AddRange(GetUsers());
        dbContext.SaveChanges();
    }

    public void Clear(AppDbContext dbContext)
    {
        dbContext.Users.RemoveRange(dbContext.Users.ToList());
        dbContext.SaveChanges();
    }

    private static User[] GetUsers() => new[]
    {
        new User
        {
            Id = 100,
            Username = "User",
            Email = UserEmailAddress,
            Password = DefaultPasswordHash,
            Role = Role.User,
            Status = AccountStatus.Active,
            EmailConfirmed = true,
            MultiFactorAuthEnabled = false
        },
        new User
        {
            Id = 101,
            Username = "Admin",
            Email = "admin@gmail.com",
            Password = DefaultPasswordHash,
            Role = Role.Admin,
            Status = AccountStatus.Active,
            EmailConfirmed = true,
            MultiFactorAuthEnabled = false
        },
        new User
        {
            Id = 102,
            Username = "Owner",
            Email = "owner@gmail.com",
            Password = DefaultPasswordHash,
            Role = Role.Owner,
            Status = AccountStatus.Active,
            EmailConfirmed = true,
            MultiFactorAuthEnabled = false
        }
    };
}