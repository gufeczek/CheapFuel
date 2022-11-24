using Domain.Entities;

namespace Application.Common.Authentication;

public interface ITokenService
{ 
    string GenerateJwtToken(User user);
    string GenerateSimpleToken();
}