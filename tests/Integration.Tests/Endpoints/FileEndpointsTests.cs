using Integration.Tests.Base;
using System.Net;
using System.Text;

namespace Integration.Tests.Endpoints;

public class FileEndpointsTests : BaseIntegrationTest
{
    private string fileContent => "file_content";
    private string fileName => "test.txt";

    private static string _baseUrl => "api/files";

    public FileEndpointsTests(IntegrationTestWebAppFactory factory)
           : base(factory)
    {
    }

    [Fact]
    public async Task Should_Successfully_EncodeFiles()
    {
        using var content = new MultipartFormDataContent();
        content.Add(new ByteArrayContent(Encoding.UTF8.GetBytes(fileContent)), "files", fileName);

        // Act
        var response = await _httpClient!.PostAsync($"{_baseUrl}/encode", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_Successfully_DecodeFiles()
    {
        using var content = new MultipartFormDataContent();
        content.Add(new ByteArrayContent(Encoding.UTF8.GetBytes(fileContent)), "files", fileName);

        // Act
        var response = await _httpClient!.PostAsync($"{_baseUrl}/decode", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
