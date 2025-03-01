namespace User_Management.Services;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

public interface IAuthService
{
    string GenerateToken();
    bool ValidateToken(string token);
}

public class AuthService : IAuthService
{
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _expiryInMinutes;

    public AuthService(IConfiguration configuration)
    {
        _secretKey =
            configuration["Jwt:Secret"]
            ?? throw new ArgumentNullException("JWT secret key is missing!");
        _issuer =
            configuration["Jwt:Issuer"]
            ?? throw new ArgumentNullException("JWT issuer is missing!");
        _audience =
            configuration["Jwt:Audience"]
            ?? throw new ArgumentNullException("JWT audience is missing!");
        _expiryInMinutes = int.TryParse(configuration["Jwt:ExpiryInMinutes"], out int expiry)
            ? expiry
            : throw new ArgumentException("Invalid JWT expiry value.");

        if (string.IsNullOrWhiteSpace(_secretKey))
        {
            throw new ArgumentException("JWT secret key cannot be empty.");
        }
    }

    public string GenerateToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(_expiryInMinutes),
            Issuer = _issuer,
            Audience = _audience,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public bool ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        try
        {
            tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                },
                out _
            );

            return true;
        }
        catch
        {
            return false;
        }
    }
}
