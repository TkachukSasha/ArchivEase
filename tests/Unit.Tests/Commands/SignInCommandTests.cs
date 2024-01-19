using Core.Commands;
using Core.Dal;
using Core.Users;
using SharedKernel.Authentication;
using SharedKernel.Errors;

namespace Unit.Tests.Commands;

public class SignInCommandTests
{
    private static readonly SignInCommand Command = new("@CrySoul1l", "test");

    private readonly SignInCommandHandler _handler;

    private readonly ArchivEaseContext _contextMock;

    private readonly IPasswordManager _passwordManagerMock;

    private readonly IJwtTokenProvider _jwtTokenProviderMock;

    public SignInCommandTests()
    {
        _contextMock = Substitute.For<ArchivEaseContext>();

        _passwordManagerMock = Substitute.For<IPasswordManager>();

        _jwtTokenProviderMock = Substitute.For<IJwtTokenProvider>();

        _handler = new SignInCommandHandler(_contextMock, _passwordManagerMock, _jwtTokenProviderMock);
    }

    [Fact]
    public async Task Should_Failed_WhenUserNotFound_ByUserName()
    {
        // Arrange
        SignInCommand invalidCommand = Command with { UserName = "test" };

        // Act
        Result result = await _handler.HandleAsync(invalidCommand, default);

        // Assert
        result.Errors.Should().Contain(UserErrors.UserIsNotFound(invalidCommand.UserName));
    }
}
