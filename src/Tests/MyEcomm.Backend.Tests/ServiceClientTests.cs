using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using MyEcomm.Backend.Tests.Helpers;
using MyEcomm.Contracts.Cart;
using MyEcomm.Contracts.Catalog;
using CartCatalogClient = MyEcomm.Cart.Api.Clients.CatalogServiceClient;
using OrderCartClient = MyEcomm.Order.Api.Clients.CartServiceClient;
using OrderCatalogClient = MyEcomm.Order.Api.Clients.CatalogServiceClient;

namespace MyEcomm.Backend.Tests;

public class ServiceClientTests
{
    [Fact]
    public async Task CartCatalogClient_GetProductAsync_ShouldReturnDtoOnSuccess()
    {
        var productId = Guid.NewGuid();
        var client = FakeHttpMessageHandler.CreateClient((_, _) => Task.FromResult(
            FakeHttpMessageHandler.Json(HttpStatusCode.OK, $$"""
            {"id":"{{productId}}","name":"Keyboard","description":"d","category":"Electronics","price":100.0,"sku":"SKU1","stockQuantity":5,"imageUrl":"img","isActive":true,"createdAt":"2026-01-01T00:00:00Z","updatedAt":"2026-01-01T00:00:00Z"}
            """)));
        var sut = new CartCatalogClient(client, NullLogger<CartCatalogClient>.Instance);

        var result = await sut.GetProductAsync(productId);

        Assert.NotNull(result);
        Assert.Equal("Keyboard", result!.Name);
    }

    [Fact]
    public async Task CartCatalogClient_GetProductAsync_ShouldReturnNullOnFailureOrException()
    {
        var nonSuccessClient = FakeHttpMessageHandler.CreateClient((_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)));
        var throwingClient = FakeHttpMessageHandler.CreateClient((_, _) => throw new HttpRequestException("boom"));
        var sut1 = new CartCatalogClient(nonSuccessClient, NullLogger<CartCatalogClient>.Instance);
        var sut2 = new CartCatalogClient(throwingClient, NullLogger<CartCatalogClient>.Instance);

        var notFound = await sut1.GetProductAsync(Guid.NewGuid());
        var error = await sut2.GetProductAsync(Guid.NewGuid());

        Assert.Null(notFound);
        Assert.Null(error);
    }

    [Fact]
    public async Task OrderCartClient_GetCartAsync_ShouldHandleSuccessAndFailure()
    {
        var successClient = FakeHttpMessageHandler.CreateClient((_, _) => Task.FromResult(
            FakeHttpMessageHandler.Json(HttpStatusCode.OK, "{\"id\":\"00000000-0000-0000-0000-000000000001\",\"guestId\":\"g1\",\"items\":[{\"id\":\"00000000-0000-0000-0000-000000000002\",\"productId\":\"00000000-0000-0000-0000-000000000003\",\"productName\":\"Mouse\",\"unitPrice\":10.0,\"quantity\":2}],\"createdAt\":\"2026-01-01T00:00:00Z\",\"updatedAt\":\"2026-01-01T00:00:00Z\"}")));
        var failClient = FakeHttpMessageHandler.CreateClient((_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)));
        var sutSuccess = new OrderCartClient(successClient, NullLogger<OrderCartClient>.Instance);
        var sutFail = new OrderCartClient(failClient, NullLogger<OrderCartClient>.Instance);

        CartDto? cart = await sutSuccess.GetCartAsync("g1");
        CartDto? missing = await sutFail.GetCartAsync("g1");

        Assert.NotNull(cart);
        Assert.Single(cart!.Items);
        Assert.Null(missing);
    }

    [Fact]
    public async Task OrderCartClient_ClearCartAsync_ShouldSwallowExceptions()
    {
        var throwingClient = FakeHttpMessageHandler.CreateClient((_, _) => throw new HttpRequestException("unavailable"));
        var sut = new OrderCartClient(throwingClient, NullLogger<OrderCartClient>.Instance);

        await sut.ClearCartAsync("guest-x");
    }

    [Fact]
    public async Task OrderCatalogClient_DecrementStockAsync_ShouldHandleFailureAndException()
    {
        var nonSuccessClient = FakeHttpMessageHandler.CreateClient((_, _) => Task.FromResult(new HttpResponseMessage(HttpStatusCode.Conflict)));
        var throwingClient = FakeHttpMessageHandler.CreateClient((_, _) => throw new HttpRequestException("down"));
        var sut1 = new OrderCatalogClient(nonSuccessClient, NullLogger<OrderCatalogClient>.Instance);
        var sut2 = new OrderCatalogClient(throwingClient, NullLogger<OrderCatalogClient>.Instance);

        await sut1.DecrementStockAsync(Guid.NewGuid(), 2);
        await sut2.DecrementStockAsync(Guid.NewGuid(), 2);
    }
}