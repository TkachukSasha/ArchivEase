using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SharedKernel.Authentication.Internal;

internal sealed class JwtTokenGenerator : IJwtTokenGenerator
{
    public string GenerateToken
    (
        string secretKey,
        string issuer,
        string audience,
        DateTimeOffset expiry,
        IEnumerable<Claim> claims
    )
    {
        SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            DateTime.UtcNow,
            expiry.DateTime,
            credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}