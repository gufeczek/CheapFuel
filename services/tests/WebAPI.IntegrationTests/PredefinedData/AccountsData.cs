using System.Linq;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Persistence;

namespace WebAPI.IntegrationTests.PredefinedData;

public class AccountsData : IPredefinedData
{
    public const int InitialUserCount = 3;
    
    public const string UserUsername = "User";
    public const string OwnerUsername = "Owner";
    public const string AdminUsername = "Admin";

    public const string Password = "Password123";

    public const string UserEmailAddress = "user@gmail.com";

    public const string InvalidUsername = "InvalidUsername";
    public const string InvalidPassword = "InvalidPassword";
    
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
            Username = "User",
            Email = UserEmailAddress,
            Password = "AQAAAAEAACcQAAAAEIx4l5FIMC2QHbCl94VCmPBY6//9LqJfoCifq8a5vxVDbfk4CMwLB6JAL0kSDgj+kA==",
            Role = Role.User,
            Status = AccountStatus.Active,
            EmailConfirmed = true,
            MultiFactorAuthEnabled = false
        },
        new User
        {
            Username = "Admin",
            Email = "admin@gmail.com",
            Password = "AQAAAAEAACcQAAAAELNzPARipTL/V1pG+HJoZnd5Rz2bZliNNXbi2qwmRYN0Hm6Xsx3Wr4ZD/W7QCeM/3g==",
            Role = Role.Admin,
            Status = AccountStatus.Active,
            EmailConfirmed = true,
            MultiFactorAuthEnabled = false
        },
        new User
        {
            Username = "Owner",
            Email = "owner@gmail.com",
            Password = "AQAAAAEAACcQAAAAEGcxtiLUVG8snLluxOTqXlPP4rzsCwAm32Eg2W0QK6BASX/OGFw4H08gG9V0cY/KtA==",
            Role = Role.Owner,
            Status = AccountStatus.Active,
            EmailConfirmed = true,
            MultiFactorAuthEnabled = false
        }
    };
}