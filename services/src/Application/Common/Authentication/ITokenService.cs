using Domain.Entities;

namespace Application.Common.Authentication;

public interface ITokenService
{ 
    string GenerateToken(User user);
}