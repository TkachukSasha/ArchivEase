using Core.Commands;
using Core.Dtos;
using Integration.Tests.Base;
using SharedKernel.Queries.Paging;
using System.Collections.Concurrent;
using System.Net.Http.Json;

namespace Integration.Tests.Endpoints;

public class UserEndpointsTests : BaseIntegrationTest
{
    private static string _baseUrl => "api/users";

    private static readonly ConcurrentDictionary<Guid, bool> _ids = new();

    public UserEndpointsTests(IntegrationTestWebAppFactory factory)
           : base(factory)
    {
    }

    [Fact]
    public async Task Should_Successfully_SignIn()
    {
        // Arrange
        var command = new SignInCommand("@CrySoul1l", "1337Master");

        // Act
        var response = await _httpClient!.PostAsJsonAsync($"{_baseUrl}/sign-in", command);

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_Successfully_SignUp()
    {
        // Arrange
        var command = new SignUpCommand("@CrySoul1l1", "1337Master1");

        // Act
        var response = await _httpClient!.PostAsJsonAsync($"{_baseUrl}/sign-up", command);
         
        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_Return_AtLeastOneUser()
    {
        // Act
        var response = await _httpClient!.GetAsync($"{_baseUrl}?page=1&results=10");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var users = await response.Content.ReadFromJsonAsync<Paged<UserDto>>();

        users!.Items.Any().Should().BeTrue();

#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
        if (users.Items.Any())
        {
            foreach (var user in users?.Items!)
                _ids.TryAdd(user!.UserId, false);
        }
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
    }

    [Fact]
    public async Task Should_Return_UserInformation_ById()
    {
        // Arrange
        Guid userId = _ids.First().Key;

        // Act
        var response = await _httpClient!.GetAsync($"{_baseUrl}/{userId}");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        var user = await response.Content.ReadFromJsonAsync<UserDto>();

        if (user is not null)
            user.UserName.Should().Be("@CrySoul1l");
    }
}
