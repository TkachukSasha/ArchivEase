using SharedKernel.Errors;

namespace Core.Users;

public static class RoleErrors
{
    public static readonly Error RoleNameMustBeProvide = Error.Validation(
        $"[{nameof(Role)}]",
        "Role name must be provide"
    );

    public static readonly Error RoleNameMustBeInExepectedRangeOfRoles = Error.Validation(
        $"[{nameof(Role)}]",
        "Role name must be in expected in range of roles"
    );

    public static readonly Error RoleNotFound = Error.NotFound(
        $"[{nameof(Role)}]",
        "Role not found by provided name"
    );
}