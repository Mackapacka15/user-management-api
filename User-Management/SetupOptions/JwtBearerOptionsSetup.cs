using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Authentication;

namespace UserManagement.SetupOptions
{
    public class JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions)
        : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;

        public void Configure(JwtBearerOptions options)
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtOptions.Issuer,
                ValidAudience = _jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(_jwtOptions.SecretKey)
                ),
                RequireExpirationTime = false,
            };
        }

        public void Configure(string? name, JwtBearerOptions options) => Configure(options);
    }
}
