namespace SharedKernel.Authentication;

[Flags]
public enum Permissions
{
    None = 0,
    ViewRoles = 1,
    ManageRoles = 2,
    ViewUsers = 4,
    ViewFiles = 8,
    ManagerUsers = 16,
    ViewPermissions = 32,
    ManagerPermissions = 64,
    ManageAccountInformation = 128,
    EncodeFile = 256,
    DecodeFile = 512,
    All = ~None
}

public static class PermissionsProvider
{
    public static IEnumerable<Permissions> GetAll()
        => Enum.GetValues(typeof(Permissions))
                .OfType<Permissions>()
                .ToList();
}