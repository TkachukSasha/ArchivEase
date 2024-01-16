using SharedKernel.Abstractions;
using SharedKernel.Authentication;
using SharedKernel.Errors;

namespace Core.Users;

public sealed class Role : Entity<RoleId>
{
    private static string[] roles =
    [
        "admin",
        "user"
    ];

    private Role() { }

    private Role(
        Guid id,
        string name,
        Permissions permissions,
        Guid userId
    ) : base(id)
    {
        Name = name;
        Permissions = permissions;
        UserId = userId;
    }

    public string Name { get; } = string.Empty;

    public Permissions Permissions { get; }

    public Guid UserId { get; }

    public User? User { get; }

    public static Result<Role> Init(
        string name,
        Permissions permissions,
        Guid userId
    ) =>
        Result.Ensure(
            name,
            (_ => roles.Contains(name), RoleErrors.RoleNameMustBeInExepectedRangeOfRoles),
            (_ => !string.IsNullOrWhiteSpace(name), RoleErrors.RoleNameMustBeProvide)
        )
        .Map(_ => new Role(new RoleId(), name, permissions, userId));
}

public sealed class RoleId : TypeId
{
    public RoleId()
        : this(Guid.NewGuid()) { }

    public RoleId(Guid value) : base(value) { }

    public static implicit operator RoleId(Guid id) => new(id);

    public static implicit operator Guid(RoleId id) => id.Value;
    public override string ToString() => Value.ToString();
}