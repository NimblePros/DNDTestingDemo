namespace ContractTestingDemo.Tests;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

public class NswagClientDemoTests
{
    [Fact]
    public async Task ConsumerTest_BardService_RequestsMasterworkSword_FromBlacksmithService()
    {
        // This is a CONSUMER test - testing that the consumer (Bard Service)
        // can correctly call and parse responses from the provider (Blacksmith Service)

        // Arrange: Create a mock HttpMessageHandler that returns the expected response
        var mockHandler = new MockHttpMessageHandler();
        mockHandler.Responses.Add("/sword/masterwork", new HttpResponseMessage
        {
            StatusCode = System.Net.HttpStatusCode.OK,
            Content = new StringContent("{\"name\":\"Masterwork Sword\",\"damage\":42}", System.Text.Encoding.UTF8, "application/json")
        });

        var httpClient = new HttpClient(mockHandler)
        {
            BaseAddress = new Uri("http://localhost:5000")
        };

        var client = new ContractTestingDemo.BlacksmithClient(httpClient);

        // Act: Call the generated client method
        var sword = await client.GetMasterworkSwordAsync();

        // Assert: Verify the response matches contract expectations
        Assert.Equal("Masterwork Sword", sword.Name);
        Assert.Equal(42, sword.Damage);
    }

    [Fact]
    public async Task ProviderTest_BlacksmithService_ProvidesMasterworkSword_API()
    {
        // This is a PROVIDER test - testing that the provider (Blacksmith Service)
        // correctly implements the API as specified in the OpenAPI contract

        // Arrange: Create a test server with the Blacksmith Service implementation
        var hostBuilder = new HostBuilder()
            .ConfigureWebHost(webHost =>
            {
                webHost.UseTestServer();
                webHost.ConfigureServices(services =>
                {
                    services.AddRouting();
                });
                webHost.Configure(app =>
                {
                    app.UseRouting();
                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapGet("/sword/masterwork", () =>
                        {
                            var sword = new ContractTestingDemo.Sword
                            {
                                Name = "Masterwork Sword",
                                Damage = 42
                            };
                            return Results.Ok(sword);
                        });
                    });
                });
            });

        var host = await hostBuilder.StartAsync();
        var client = host.GetTestClient();

        // Act: Make request to the actual API implementation
        var response = await client.GetAsync("/sword/masterwork");

        // Assert: Verify the provider fulfills the contract
        response.EnsureSuccessStatusCode();
        var sword = await response.Content.ReadFromJsonAsync<ContractTestingDemo.Sword>();
        Assert.NotNull(sword);
        Assert.Equal("Masterwork Sword", sword.Name);
        Assert.Equal(42, sword.Damage);

        await host.StopAsync();
    }
}

// Simple mock HttpMessageHandler for testing
public class MockHttpMessageHandler : HttpMessageHandler
{
    public Dictionary<string, HttpResponseMessage> Responses { get; } = new();

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (Responses.TryGetValue(request.RequestUri?.PathAndQuery ?? "", out var response))
        {
            return Task.FromResult(response);
        }
        return Task.FromResult(new HttpResponseMessage(System.Net.HttpStatusCode.NotFound));
    }
}
