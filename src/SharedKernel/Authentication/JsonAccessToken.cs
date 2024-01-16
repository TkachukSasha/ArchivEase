using System.Security.Claims;

namespace SharedKernel.Authentication;

public sealed class JsonAccessToken
{
    public JsonAccessToken
    (
        string token,
        long expires,
        string userGid,
        IEnumerable<Claim>? claims)
    {
        Token = token;
        Expires = expires;
        UserGid = userGid;
        Claims = claims;
    }

    public string Token { get; }

    public long Expires { get; }

    public string UserGid { get; }

    public IEnumerable<Claim>? Claims { get; }
}