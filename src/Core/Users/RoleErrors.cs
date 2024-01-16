using SharedKernel.Errors;

namespace Core.Users;

public static class RoleErrors
{
    public static readonly Error RoleNameMustBeProvide = new Error(
        $"[{nameof(Role)}]",
        "Role name must be provide"
    );

    public static readonly Error RoleNameMustBeInExepectedRangeOfRoles = new Error(
         $"[{nameof(Role)}]",
        "Role name must be in expected in range of roles"
    );
}