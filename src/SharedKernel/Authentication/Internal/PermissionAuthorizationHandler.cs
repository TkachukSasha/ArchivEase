using Microsoft.AspNetCore.Authorization;

namespace SharedKernel.Authentication.Internal;

internal sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync
    (
        AuthorizationHandlerContext context,
        PermissionAuthorizationRequirement requirement
    )
    {
        var permissionClaim = context.User.FindFirst(
            c => c.Type == CustomClaims.Permissions
        );

        if (permissionClaim is null)
            return Task.CompletedTask;

        if (!int.TryParse(permissionClaim.Value, out int permissionClaimValue))
            return Task.CompletedTask;

        var userPermissions = (Permissions)permissionClaimValue;

        if (userPermissions == Permissions.All)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        if ((userPermissions & requirement.Permissions) != 0)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
}