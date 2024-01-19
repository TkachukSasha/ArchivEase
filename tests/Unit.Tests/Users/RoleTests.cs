using Core.Users;
using SharedKernel.Authentication;
using SharedKernel.Errors;

namespace Unit.Tests.Users;

public class RoleTests
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Failed_WhenRoleName_IsNullOrWhiteSpace(string roleName)
    {
        Result<Role> role = Role.Init(roleName, Permissions.All);

        role.IsFailure.Should().BeTrue();
        role.Errors.Should().Contain(RoleErrors.RoleNameMustBeInExepectedRangeOfRoles);
        role.Errors.Should().Contain(RoleErrors.RoleNameMustBeProvide);
    }
}