using SharedKernel.Errors;
namespace Core.Users;

public sealed class UserRole
{
    private UserRole() { }

    private UserRole(
        Guid userId,
        Guid roleId
    )
    {
        UserId = userId;
        RoleId = roleId;
    }

    public Guid UserId { get; }

    public Guid RoleId { get; }

    public static Result<UserRole> Init(
        Guid userId,
        Guid roleId
    ) =>
        Result.Ensure(
            (userId, roleId),
            (_ => userId != Guid.Empty, UserRoleErrors.UserIdMustBeProvide),
            (_ => roleId != Guid.Empty, UserRoleErrors.RoleIdMustBeProvide)
        )
        .Map(_ => new UserRole(userId, roleId));
}