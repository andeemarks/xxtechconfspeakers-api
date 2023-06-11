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

    [Fact]
    public async Task Get_MainEndpointEnabled()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/TechConfSummary");

        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType!.ToString());
    }
}