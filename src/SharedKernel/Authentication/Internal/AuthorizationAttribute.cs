namespace SharedKernel.Authentication.Internal;

public class AuthorizationAttribute : Microsoft.AspNetCore.Authorization.AuthorizeAttribute
{
    public AuthorizationAttribute() { }

    public AuthorizationAttribute(string policy) : base(policy) { }

    public AuthorizationAttribute(Permissions permissions) => Permissions = permissions;

    public Permissions Permissions
    {
        get =>
            !string.IsNullOrEmpty(Policy)
                ? PolicyNameHelper.GetPermissionsFrom(Policy)
                : Permissions.None;
        set =>
            Policy = value != Permissions.None
                ? PolicyNameHelper.GeneratePolicyNameFor(value)
                : string.Empty;
    }
}