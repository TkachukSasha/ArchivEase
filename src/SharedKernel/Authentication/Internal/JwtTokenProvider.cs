using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Authentication.Internal;

internal sealed class JwtTokenProvider : IJwtTokenProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public JwtTokenProvider
    (
        JwtOptions jwtOptions,
        IJwtTokenGenerator tokenGenerator
    )
    {
        _jwtOptions = jwtOptions;
        _tokenGenerator = tokenGenerator;
    }

    public JsonAccessToken CreateToken
    (
        string userGid,
        string userName,
        string permissions
    )
    {
        var now = TimeProvider.System.GetUtcNow();

        var jwtClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userGid),
            new Claim(JwtRegisteredClaimNames.Sub, userGid),
            new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString())
        };

        if (!string.IsNullOrWhiteSpace(permissions))
        {
            jwtClaims.Add(new Claim(CustomClaims.Permissions, permissions));
        }

        if (!string.IsNullOrWhiteSpace(userName))
        {
            jwtClaims.Add(new Claim(ClaimTypes.Name, userName));
        }

        var expiry = now.AddMinutes(_jwtOptions.AccessTokenExpiryMinutes * 3);

        var acess_token = _tokenGenerator.GenerateToken
        (
            _jwtOptions.SecretKey,
            _jwtOptions.Issuer,
            _jwtOptions.Audience,
            expiry,
            jwtClaims
        );

        return new JsonAccessToken
        (
            acess_token,
            expiry.ToTimestamp(),
            userGid,
            jwtClaims
        );
    }
}