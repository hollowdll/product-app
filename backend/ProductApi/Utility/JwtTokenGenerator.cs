using System.Security.Claims;
using ProductApi.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ProductApi.Utility;

public static class JwtTokenGenerator
{
    /// <summary>
    /// Generates a new JSON Web Token.
    /// </summary>
    public static string GenerateToken(AppJwtConfig appJwtConfig, ClaimsIdentity claimsIdentity, double expirationMinutes)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appJwtConfig.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = claimsIdentity,
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            Issuer = appJwtConfig.Issuer,
            Audience = appJwtConfig.Audience,
            SigningCredentials = creds
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
