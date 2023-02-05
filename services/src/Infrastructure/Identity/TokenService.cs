using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Common.Authentication;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Identity;

public class TokenService : ITokenService
{
    private const string SimpleTokenAllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    
    private readonly AuthenticationSettings _authenticationSettings;

    public TokenService(AuthenticationSettings authenticationSettings)
    {
        _authenticationSettings = authenticationSettings;
    }
    
    public string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new("Id", user.Id.ToString()),
            new(ClaimTypes.NameIdentifier, user.Username!),
            new(ClaimTypes.Role, user.Role.ToString())
        };
        var identity = new ClaimsIdentity(claims);
        var expires = DateTime.Now.AddDays((double) _authenticationSettings.ExpireDays!);
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.Secret!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
        {
            Issuer = _authenticationSettings.Issuer,
            Audience = _authenticationSettings.Audience,
            Subject = identity,
            Expires = expires,
            SigningCredentials = credentials
        };

        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        SecurityToken token = handler.CreateToken(descriptor);
        return handler.WriteToken(token);
    }

    public string GenerateSimpleToken()
    {
        var chars = Enumerable.Repeat(SimpleTokenAllowedChars, 6)
            .Select(token => token[RandomNumberGenerator.GetInt32(SimpleTokenAllowedChars.Length)])
            .ToArray();
        return new string(chars);
    }
}