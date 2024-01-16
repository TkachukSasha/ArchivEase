namespace SharedKernel.Authentication;

public interface IJwtTokenProvider
{
    JsonAccessToken CreateToken
    (
        string userGid,
        string userName,
        string permissions
    );
}