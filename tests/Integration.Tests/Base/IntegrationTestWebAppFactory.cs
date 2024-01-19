using Core.Dal;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Integration.Tests.Base;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    public HttpClient? Client { get; private set; }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
    }

    public Task InitializeAsync()
    {
        Client = CreateClient();

        return Task.CompletedTask;
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        Client?.Dispose();

        return Task.CompletedTask;
    }
}