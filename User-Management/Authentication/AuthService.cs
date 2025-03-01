namespace UserManagement.Services;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Authentication;

public interface IAuthService
{
    string GenerateToken();
}

public class AuthService(IOptions<JwtOptions> options) : IAuthService
{
    private readonly JwtOptions _jwtOptions = options.Value;
    private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(1);

    public string GenerateToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtOptions.SecretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.Add(TokenLifetime),
            Issuer = _jwtOptions.Issuer,
            Audience = _jwtOptions.Audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var JWT = tokenHandler.WriteToken(token);
        return JWT;
    }
}
