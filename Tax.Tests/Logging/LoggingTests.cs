using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Tax.Tests.Logging;

public class LoggingTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly InMemoryLoggerProvider _loggerProvider = new();

    public LoggingTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // remove existing logging providers and add test provider
                services.AddSingleton<Microsoft.Extensions.Logging.ILoggerProvider>(_loggerProvider);
            });
        });
    }

    [Fact]
    public async Task Api_Returns_TraceId_Header_When_UserId_Header_Present()
    {
        using var client = _factory.CreateClient();
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/tax?taxableIncome=1000");
        request.Headers.Add("X-User-Id", "test-user-123");

        var resp = await client.SendAsync(request);
        resp.EnsureSuccessStatusCode();

        Assert.True(resp.Headers.Contains("X-Trace-Id"));

        // ensure that at least one captured log scope contains the UserId
        var found = false;
        foreach (var entry in _loggerProvider.Logs)
        {
            foreach (var scope in entry.Scopes)
            {
                if (scope is System.Collections.Generic.KeyValuePair<string, object?> kvp && kvp.Key == "UserId" && kvp.Value?.ToString() == "test-user-123")
                {
                    found = true;
                    break;
                }
            }
            if (found) break;
        }

        Assert.True(found, "Expected at least one log scope to include UserId");
    }
}
