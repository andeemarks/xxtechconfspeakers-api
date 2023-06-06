using Microsoft.AspNetCore.Mvc.Testing;

namespace TechConfSpeakersController.IntegrationTests;

// <snippet1>
public class TechConfSpeakersControllerTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public TechConfSpeakersControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Theory]
    [InlineData("/swagger")]
    public async Task Get_SwaggerEndpointEnabled(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Theory]
    [InlineData("/TechConfSpeakers")]
    public async Task Get_MainEndpointEnabled(string url)
    {
        var client = _factory.CreateClient();
        var response = await client.GetAsync(url);

        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
    }
}