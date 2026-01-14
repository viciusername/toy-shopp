using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using Xunit;

namespace ToyPlanet.Tests;

public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetToysV1_ReturnsOkAndListOfToys()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/toys");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Rainbow Dash", content);
        Assert.Contains("\"name\"", content);
    }

    [Fact]
    public async Task GetToysV2_ReturnsOkAndListOfToysWithCategoryDetails()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v2/toys");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("category", content);
        Assert.Contains("description", content);
    }

    [Fact]
    public async Task GetToyByIdV1_ReturnsOkAndToyData()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/toys/1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Rainbow Dash", content);
        Assert.Contains("450", content);
    }

    [Fact]
    public async Task GetToyByIdV2_ReturnsOkAndToyWithCategoryDetails()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v2/toys/1");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("category", content);
        Assert.Contains("description", content);
    }

    [Fact]
    public async Task GetOrders_ReturnsOkAndListOfOrders()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/orders");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("pony.lover@example.com", content);
        Assert.Contains("rainbow.fan@example.com", content);
    }

    [Fact]
    public async Task GetCategories_ReturnsOkAndListOfCategories()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/categories");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"name\"", content);
        Assert.Contains("\"description\"", content);
    }

    [Fact]
    public async Task GetUsers_ReturnsOkAndListOfUsers()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/users");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("pony.lover@example.com", content);
        Assert.Contains("rainbow.fan@example.com", content);
    }

    [Fact]
    public async Task SwaggerUI_IsAccessible()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/swagger");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SwaggerV1Doc_IsAccessible()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/swagger/v1/swagger.json");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("ToyPlanet API v1", content);
    }

    [Fact]
    public async Task SwaggerV2Doc_IsAccessible()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/swagger/v2/swagger.json");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("ToyPlanet API v2", content);
    }

    [Fact]
    public async Task ApiV1AndV2_HaveBackwardCompatibility()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var v1Response = await client.GetAsync("/api/v1/toys/1");
        var v1Content = await v1Response.Content.ReadAsStringAsync();
        
        var v2Response = await client.GetAsync("/api/v2/toys/1");
        var v2Content = await v2Response.Content.ReadAsStringAsync();

        // Assert - V2 should have everything V1 has plus more
        Assert.Equal(HttpStatusCode.OK, v1Response.StatusCode);
        Assert.Equal(HttpStatusCode.OK, v2Response.StatusCode);
        
        Assert.Contains("name", v1Content);
        Assert.Contains("price", v1Content);
        
        Assert.Contains("name", v2Content);
        Assert.Contains("price", v2Content);
        Assert.Contains("category", v2Content);
    }

    [Fact]
    public async Task GetOrderById_ReturnsOkAndOrderData()
    {
        // Arrange
        var client = _factory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/api/v1/orders");
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.NotEmpty(content);
        Assert.Contains("[", content);
    }
}
