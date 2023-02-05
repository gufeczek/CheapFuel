using System;
using System.Linq;
using Domain.Enums;
using Infrastructure.Persistence;

namespace WebAPI.IntegrationTests.PredefinedData;

public class UserQueryControllerData : IPredefinedData
{
    public const int InitialUsersCount = 5;

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

    private Domain.Entities.User[] GetUsers() => new[]
    {
        new Domain.Entities.User()
        {
            Id = 107890,
            Username = "Grzesio",
            CreatedAt = new DateTime(2022, 11, 1),
            Email = "grzesio@gmail.com",
            EmailConfirmed = true,
            MultiFactorAuthEnabled = true,
            Password = "Password123",
            Role = Role.User
        },
        new Domain.Entities.User()
        {
            Id = 1079781,
            Username = "Kazio",
            CreatedAt = new DateTime(2022, 12, 31),
            Email = "kazio@gmail.com",
            EmailConfirmed = true,
            MultiFactorAuthEnabled = true,
            Password = "Password123",
            Role = Role.User
        }
    };
}