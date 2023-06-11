using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests;

// <snippet1>
public class TechConfSummaryControllerTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public TechConfSummaryControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/TechConfSummary")]
    public async Task Get_MainEndpointEnabled(string url)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType!.ToString());
    }
}