using System;
using System.Linq;
using Domain.Entities;
using Domain.Entities.Tokens;
using Domain.Enums;
using Infrastructure.Persistence;

namespace WebAPI.IntegrationTests.PredefinedData;

public class AccountAdditionalData : IPredefinedData
{
    public const int InitialUserCount = 7;

    private const long UserWithoutConfirmedEmailId = 200;
    public const string UserWithoutConfirmedEmailUsername = "UserWithoutConfEmail";

    public const long UserWithExpiredEmailConfirmationTokenId = 201;
    public const string UserWithExpiredEmailConfirmationTokenUsername = "UserWithExpiredEmailToken";
    
    public const long UserWithoutEmailConfirmationTokenId = 202;
    public const string UserWithoutEmailConfirmationTokenUsername = "UserWithoutEmailToken";

    public const string UserWithVerifiedEmailAddressUsername = "UserWithVerifiedEmail";

    public const long UserWithoutPasswordResetTokenId = 204;
    public const string UserWithoutPasswordResetTokenUsername = "UserWithoutPassResetToken";
    public const string UserWithoutPasswordResetTokenEmail = $"{UserWithoutPasswordResetTokenUsername}@gmail.com";
    
    public const long UserWithPasswordResetTokenId = 205;
    public const string UserWithPasswordResetTokenUsername = "UserWithPassResetToken";
    public const string UserWithPasswordResetTokenEmail = $"{UserWithPasswordResetTokenUsername}@gmail.com";

    public const long UserWithExpiredPasswordResetTokenId = 206;
    public const string UserWithExpiredPasswordResetTokenUsername = "UserWithExpiredPassResetToken";
    public const string UserWithExpiredPasswordResetTokenEmail = $"{UserWithExpiredPasswordResetTokenUsername}@gmail.com";
    
    public const int InitialEmailTokenCount = 2;
    
    public const long ActiveEmailTokenId = 100;
    public const string ActiveEmailTokenCode = "ABCDEF";

    public const long ExpiredEmailTokenId = 101;
    public const string InvalidEmailTokenCode = "123456";

    public const int InitialPasswordResetTokenCount = 2;

    public const long ActivePasswordResetTokenId = 100;
    public const string ActivePasswordResetTokenCode = "ABCDEF";
    public const string InvalidPasswordResetTokenCode = "123456";
    
    public const long ExpiredPasswordResetTokenId = 101;

    public void Seed(AppDbContext dbContext)
    {
        dbContext.Users.AddRange(GetUsers());
        dbContext.EmailVerificationTokens.AddRange(GetEmailVerificationTokens());
        dbContext.PasswordResetTokens.AddRange(GetPasswordResetTokens());
        dbContext.SaveChanges();
    }

    public void Clear(AppDbContext dbContext)
    {
        dbContext.PasswordResetTokens.RemoveRange(dbContext.PasswordResetTokens.ToList());
        dbContext.EmailVerificationTokens.RemoveRange(dbContext.EmailVerificationTokens.ToList());
        dbContext.Users.RemoveRange(dbContext.Users.ToList());
        dbContext.SaveChanges();
    }

    private static User[] GetUsers() => new[]
    {
        new User
        {
            Id = UserWithoutConfirmedEmailId,
            Username = UserWithoutConfirmedEmailUsername,
            Email = $"{UserWithoutConfirmedEmailUsername}@gmail.com",
            Password = AccountsData.DefaultPasswordHash,
            Role = Role.User,
            Status = AccountStatus.Active,
            EmailConfirmed = false,
            MultiFactorAuthEnabled = false
        },
        new User
        {
            Id = UserWithExpiredEmailConfirmationTokenId,
            Username = UserWithExpiredEmailConfirmationTokenUsername,
            Email = $"{UserWithExpiredEmailConfirmationTokenUsername}@gmail.com",
            Password = AccountsData.DefaultPasswordHash,
            Role = Role.User,
            Status = AccountStatus.Active,
            EmailConfirmed = false,
            MultiFactorAuthEnabled = false
        },
        new User
        {
            Id = UserWithoutEmailConfirmationTokenId,
            Username = UserWithoutEmailConfirmationTokenUsername,
            Email = $"{UserWithoutEmailConfirmationTokenUsername}@gmail.com",
            Password = AccountsData.DefaultPasswordHash,
            Role = Role.User,
            Status = AccountStatus.Active,
            EmailConfirmed = false,
            MultiFactorAuthEnabled = false
        },
        new User
        {
            Id = 203,
            Username = UserWithVerifiedEmailAddressUsername,
            Email = $"{UserWithVerifiedEmailAddressUsername}@gmail.com",
            Password = AccountsData.DefaultPasswordHash,
            Role = Role.User,
            Status = AccountStatus.Active,
            EmailConfirmed = true,
            MultiFactorAuthEnabled = false
        },
        new User
        {
            Id = UserWithoutPasswordResetTokenId,
            Username = UserWithoutPasswordResetTokenUsername,
            Email = UserWithoutPasswordResetTokenEmail,
            Password = AccountsData.DefaultPasswordHash,
            Role = Role.User,
            Status = AccountStatus.Active,
            EmailConfirmed = true,
            MultiFactorAuthEnabled = false
        },
        new User
        {
            Id = UserWithPasswordResetTokenId,
            Username = UserWithPasswordResetTokenUsername,
            Email = UserWithPasswordResetTokenEmail,
            Password = AccountsData.DefaultPasswordHash,
            Role = Role.User,
            Status = AccountStatus.Active,
            EmailConfirmed = true,
            MultiFactorAuthEnabled = false
        },
        new User
        {
            Id = UserWithExpiredPasswordResetTokenId,
            Username = UserWithExpiredPasswordResetTokenUsername,
            Email = UserWithExpiredPasswordResetTokenEmail,
            Password = AccountsData.DefaultPasswordHash,
            Role = Role.User,
            Status = AccountStatus.Active,
            EmailConfirmed = true,
            MultiFactorAuthEnabled = false
        }
    };

    private static EmailVerificationToken[] GetEmailVerificationTokens() => new[]
    {
        new EmailVerificationToken
        {
            Id = 100,
            Token = ActiveEmailTokenCode,
            Count = 0,
            CreatedAt = DateTime.UtcNow,
            UserId = UserWithoutConfirmedEmailId
        },
        new EmailVerificationToken
        {
            Id = 101,
            Token = ActiveEmailTokenCode,
            Count = 0,
            CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(121)),
            UserId = UserWithExpiredEmailConfirmationTokenId
        }
    };

    private static PasswordResetToken[] GetPasswordResetTokens() => new[]
    {
        new PasswordResetToken
        {
            Id = ActivePasswordResetTokenId,
            Token = ActivePasswordResetTokenCode,
            Count = 0,
            CreatedAt = DateTime.UtcNow,
            UserId = UserWithPasswordResetTokenId
        },
        new PasswordResetToken
        {
            Id = ExpiredPasswordResetTokenId,
            Token = ActivePasswordResetTokenCode,
            Count = 0,
            CreatedAt = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(31)),
            UserId = UserWithExpiredPasswordResetTokenId
        }
    };
}