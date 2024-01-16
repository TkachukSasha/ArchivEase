namespace SharedKernel.Authentication;

[Flags]
public enum Permissions
{
    None = 0,
    ViewRoles = 1,
    ManageRoles = 2,
    ViewUsers = 4,
    ManagerUsers = 8,
    ViewPermissions = 16,
    ManagerPermissions = 32,
    ManageAccountInformation = 64,
    EncodeFile = 128,
    DecodeFile = 256,
    All = ~None
}

public static class PermissionsProvider
{
    public static IEnumerable<Permissions> GetAll()
        => Enum.GetValues(typeof(Permissions))
                .OfType<Permissions>()
                .ToList();
}