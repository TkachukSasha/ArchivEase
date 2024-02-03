using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace SharedKernel.Authentication.Internal;

internal class FileAuthorizationHandler : AuthorizationHandler<FileAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FileAuthorizationRequirement requirement)
    {
        if (!context.User.Identity!.IsAuthenticated)
        {
            if (context.Resource is List<IFormFile> files && files.Count <= requirement.MaxFilesForAnonymous)
            {
                context.Succeed(requirement);
            }
        }
        else
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

            if ((userPermissions & Permissions.EncodeFiles | Permissions.DecodeFiles) != 0)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
        }

        return Task.CompletedTask;
    }
}