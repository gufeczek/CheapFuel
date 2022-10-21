using Application.Common.Authentication;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class UserPasswordHasher : IUserPasswordHasher
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserPasswordHasher(IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }
    
    public string HashPassword(string password, User user)
    {
        return _passwordHasher.HashPassword(user, password);
    }
}