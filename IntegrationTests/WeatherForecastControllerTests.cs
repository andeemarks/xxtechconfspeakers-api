using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace WeatherForecastController.IntegrationTests;

// <snippet1>
public class WeatherForecastControllerTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public WeatherForecastControllerTests(WebApplicationFactory<Program> factory)
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
    [InlineData("/WeatherForecast")]
    public async Task Get_MainEndpointEnabled(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json; charset=utf-8",
            response.Content.Headers.ContentType.ToString());
    }
}