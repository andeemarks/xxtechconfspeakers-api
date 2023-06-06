using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests;

public class TestWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
        });

        return base.CreateHost(builder);
    }
}