using Core.Users;
using SharedKernel.Errors;

namespace Unit.Tests.Users;

public class UserTests
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Failed_WhenUserName_IsNullOrWhiteSpace(string userName)
    {
        Result<User> user = User.Init(userName, "password");

        user.IsFailure.Should().BeTrue();
        user.Errors.Should().Contain(UserErrors.UserNameMustBeProvide);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Should_Failed_WhenPassword_IsNullOrWhiteSpace(string password)
    {
        Result<User> user = User.Init("test_user_name", password);

        user.IsFailure.Should().BeTrue();
        user.Errors.Should().Contain(UserErrors.PasswordMustBeProvide);
    }
}