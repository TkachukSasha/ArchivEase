namespace SharedKernel.Authentication.Internal;

internal static class PolicyNameHelper
{
    internal static string Prefix = "Permissions_";

    internal static bool IsValidPolicyName(string policyName)
        => policyName != null && policyName.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase);

    internal static string GeneratePolicyNameFor(Permissions permissions)
        => $"{Prefix}{(int)permissions}";

    internal static Permissions GetPermissionsFrom(string policyName)
    {
        var permissionValue = int.Parse(policyName.Split("_")[1]);

        return (Permissions)permissionValue;
    }
}