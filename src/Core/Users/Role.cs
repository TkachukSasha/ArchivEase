using SharedKernel.Abstractions;
using SharedKernel.Authentication;
using SharedKernel.Errors;

namespace Core.Users;

public sealed class Role : Entity<RoleId>
{
    public static string AdminRole => "admin";

    public static string DefaultRole => "user";


    private static string[] Roles =
    [
        "admin",
        "user"
    ];

    private Role() { }

    private Role(
        Guid id,
        string name,
        Permissions permissions
    ) : base(id)
    {
        Name = name;
        Permissions = permissions;
    }

    public string Name { get; } = DefaultRole;

    public Permissions Permissions { get; }

    public static Result<Role> Init(
        string name,
        Permissions permissions
    ) =>
        Result.Ensure(
            name,
            (_ => Roles.Contains(name), RoleErrors.RoleNameMustBeInExepectedRangeOfRoles),
            (_ => !string.IsNullOrWhiteSpace(name), RoleErrors.RoleNameMustBeProvide)
        )
        .Map(_ => new Role(new RoleId(), name, permissions));
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