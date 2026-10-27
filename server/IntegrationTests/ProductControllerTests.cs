namespace IntegrationTests.Controllers;

using System.Net;
using System.Net.Http.Json;
using IntegrationTests.Fixtures;
using Infrastructure.Entities;
using Service.DTOs;
using Xunit;

public class ProductsControllerTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task Post_And_Get_Products_ReturnsCreatedAndSuccess()
    {
        // Arrange
        var request = new CreateProductRequest("Mechanical Keyboard", 799.00m);

        // Act: Send POST request to endpoint
        var postResponse = await _client.PostAsJsonAsync("/api/products", request);
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);

        // Act: Send GET request to verify persistence
        var getResponse = await _client.GetAsync("/api/products");
        var products = await getResponse.Content.ReadFromJsonAsync<List<Product>>();

        // Assert: Validate end-to-end correctness
        Assert.NotNull(products);
        Assert.Single(products);
        Assert.Equal("Mechanical Keyboard", products[0].Name);
    }

    [Fact]
    public async Task OpenApiDocument_ReturnsSuccessAndValidJson()
    {
        // Act: Verify OpenAPI endpoint contract
        var response = await _client.GetAsync("/openapi/v1.json");

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("application/json", response.Content.Headers.ContentType?.MediaType);
    }
}