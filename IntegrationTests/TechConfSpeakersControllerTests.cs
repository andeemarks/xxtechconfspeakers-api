using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests;

// <snippet1>
public class TechConfSpeakersControllerTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public TechConfSpeakersControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_SwaggerEndpointEnabled()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/swagger");

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task Get_MainEndpointEnabled()
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync("/TechConfSpeakers");

        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType!.ToString());
    }
}