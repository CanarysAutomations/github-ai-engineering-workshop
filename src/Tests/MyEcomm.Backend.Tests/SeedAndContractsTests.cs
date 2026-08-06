using MyEcomm.Catalog.Api.Repositories;
using MyEcomm.Catalog.Api.Seed;
using MyEcomm.Contracts.Cart;
using MyEcomm.Contracts.Catalog;

namespace MyEcomm.Backend.Tests;

public class SeedAndContractsTests
{
    [Fact]
    public void ProductSeeder_ShouldSeedTenProducts()
    {
        var repo = new InMemoryProductRepository();

        ProductSeeder.SeedProducts(repo);
        var (items, totalCount) = repo.GetAll(null, null, 1, 100);

        Assert.Equal(10, totalCount);
        Assert.Equal(10, items.Count);
        Assert.All(items, p =>
        {
            Assert.True(p.IsActive);
            Assert.Contains("https://picsum.photos/seed/", p.ImageUrl);
        });
    }

    [Fact]
    public void ContractComputedProperties_ShouldCalculateTotalsAndStockState()
    {
        var cart = new CartDto
        {
            Items =
            {
                new CartItemDto { UnitPrice = 10m, Quantity = 2 },
                new CartItemDto { UnitPrice = 7.5m, Quantity = 1 },
            }
        };

        var inStock = new ProductDto { StockQuantity = 3 };
        var outOfStock = new ProductDto { StockQuantity = 0 };

        Assert.Equal(27.5m, cart.Total);
        Assert.True(inStock.InStock);
        Assert.False(outOfStock.InStock);
        Assert.Equal(20m, cart.Items[0].LineTotal);
    }
}