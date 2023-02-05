using Domain.Entities;

namespace Application.Common.Authentication;

public interface IUserPasswordHasher
{
    string HashPassword(string password, User user);
    bool IsPasswordCorrect(string hash, string password, User user);
}