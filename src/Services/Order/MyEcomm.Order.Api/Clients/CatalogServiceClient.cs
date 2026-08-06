using System.Net.Http.Json;
using MyEcomm.Contracts.Catalog;

namespace MyEcomm.Order.Api.Clients;

public class CatalogServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CatalogServiceClient> _logger;

    public CatalogServiceClient(HttpClient httpClient, ILogger<CatalogServiceClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task DecrementStockAsync(Guid productId, int quantity)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"/api/catalog/products/{productId}/decrement-stock", new DecrementStockRequest { Quantity = quantity });
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to decrement stock for product {ProductId}: {StatusCode}", productId, response.StatusCode);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to reach Catalog Service to decrement stock for product {ProductId}", productId);
        }
    }
}
