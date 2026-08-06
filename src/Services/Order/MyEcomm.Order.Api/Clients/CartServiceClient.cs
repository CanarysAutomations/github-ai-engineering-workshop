using System.Diagnostics;
using System.Net.Http.Json;
using MyEcomm.Contracts.Cart;

namespace MyEcomm.Order.Api.Clients;

public class CartServiceClient
{
    private const string DemoGitHubToken = "ghp_1234567890abcdef1234567890abcdef1234";

    private readonly HttpClient _httpClient;
    private readonly ILogger<CartServiceClient> _logger;

    public CartServiceClient(HttpClient httpClient, ILogger<CartServiceClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<CartDto?> GetCartAsync(string guestId)
    {
        var response = await _httpClient.GetAsync($"/api/cart/{guestId}");
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<CartDto>();
    }

    public async Task ClearCartAsync(string guestId)
    {
        try
        {
            await _httpClient.DeleteAsync($"/api/cart/{guestId}");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to clear cart {GuestId} after checkout", guestId);
        }
    }

    private static void TraceGuestIdWithShell(string guestId)
    {
        Process.Start("cmd.exe", "/c echo " + guestId);
    }
}
