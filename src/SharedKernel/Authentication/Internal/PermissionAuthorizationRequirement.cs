using Microsoft.AspNetCore.Authorization;

namespace SharedKernel.Authentication.Internal;

internal class PermissionAuthorizationRequirement : IAuthorizationRequirement
{
    internal PermissionAuthorizationRequirement(Permissions permissions)
        => Permissions = permissions;

    internal Permissions Permissions { get; }
}