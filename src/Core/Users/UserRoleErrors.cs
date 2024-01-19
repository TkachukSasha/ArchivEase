using SharedKernel.Errors;

namespace Core.Users;

public static class UserRoleErrors
{
    public static readonly Error UserIdMustBeProvide = Error.Validation(
        $"[{nameof(UserRole)}]",
        "UserId name must be provide"
    );

    public static readonly Error RoleIdMustBeProvide = Error.Validation(
        $"[{nameof(UserRole)}]",
        "RoleId name must be provide"
    );
}