using System.Security.Claims;

namespace SharedKernel.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken
    (
        string secretKey,
        string issuer,
        string audience,
        DateTimeOffset expiry,
        IEnumerable<Claim> claims
    );
}