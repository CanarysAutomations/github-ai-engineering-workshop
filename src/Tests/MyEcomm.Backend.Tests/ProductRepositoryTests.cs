using MyEcomm.Catalog.Api.Models;
using MyEcomm.Catalog.Api.Repositories;

namespace MyEcomm.Backend.Tests;

public class ProductRepositoryTests
{
    [Fact]
    public void Add_ShouldAssignIdentityAndDefaults()
    {
        var repo = new InMemoryProductRepository();
        var product = new Product { Name = "Mouse", Category = "Electronics", Price = 25m, Sku = "ELEC-100", StockQuantity = 10 };

        var created = repo.Add(product);

        Assert.NotEqual(Guid.Empty, created.Id);
        Assert.True(created.IsActive);
        Assert.True(created.CreatedAt <= DateTime.UtcNow);
        Assert.True(created.UpdatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void GetAll_ShouldFilterSearchAndPaginate()
    {
        var repo = new InMemoryProductRepository();
        repo.Seed(new[]
        {
            new Product { Id = Guid.NewGuid(), Name = "Alpha Mouse", Category = "Electronics", IsActive = true },
            new Product { Id = Guid.NewGuid(), Name = "Beta Keyboard", Category = "Electronics", IsActive = true },
            new Product { Id = Guid.NewGuid(), Name = "Gamma Shirt", Category = "Apparel", IsActive = true },
            new Product { Id = Guid.NewGuid(), Name = "Hidden Product", Category = "Electronics", IsActive = false },
        });

        var (items, totalCount) = repo.GetAll("electronics", "a", 1, 1);

        Assert.Equal(2, totalCount);
        Assert.Single(items);
        Assert.Equal("Alpha Mouse", items[0].Name);
    }

    [Fact]
    public void GetAll_ShouldApplyDefaultPageAndPageSize_WhenInvalid()
    {
        var repo = new InMemoryProductRepository();
        repo.Seed(Enumerable.Range(1, 25).Select(i => new Product
        {
            Id = Guid.NewGuid(),
            Name = $"P{i:00}",
            Category = "Electronics",
            IsActive = true
        }));

        var (items, totalCount) = repo.GetAll(null, null, 0, 0);

        Assert.Equal(25, totalCount);
        Assert.Equal(20, items.Count);
        Assert.Equal("P01", items[0].Name);
    }

    [Fact]
    public void GetAll_ShouldReturnEmptyItems_WhenPageIsOutOfRange()
    {
        var repo = new InMemoryProductRepository();
        repo.Seed(Enumerable.Range(1, 5).Select(i => new Product
        {
            Id = Guid.NewGuid(),
            Name = $"Item-{i}",
            Category = "Electronics",
            IsActive = true
        }));

        var (items, totalCount) = repo.GetAll(null, null, page: 3, pageSize: 3);

        Assert.Equal(5, totalCount);
        Assert.Empty(items);
    }

    [Fact]
    public void Update_ShouldReturnNull_WhenProductMissing()
    {
        var repo = new InMemoryProductRepository();

        var updated = repo.Update(Guid.NewGuid(), new Product { Name = "Updated" });

        Assert.Null(updated);
    }

    [Fact]
    public void Update_ShouldMutateExistingProduct()
    {
        var repo = new InMemoryProductRepository();
        var created = repo.Add(new Product { Name = "Original", Category = "Electronics", Price = 10m, Sku = "SKU-1", StockQuantity = 3 });
        var beforeUpdatedAt = created.UpdatedAt;

        var updated = repo.Update(created.Id, new Product
        {
            Name = "Updated",
            Description = "desc",
            Category = "Apparel",
            Price = 12m,
            Sku = "SKU-2",
            StockQuantity = 9,
            ImageUrl = "img",
            IsActive = false
        });

        Assert.NotNull(updated);
        Assert.Equal("Updated", updated!.Name);
        Assert.Equal("Apparel", updated.Category);
        Assert.Equal(12m, updated.Price);
        Assert.Equal(9, updated.StockQuantity);
        Assert.False(updated.IsActive);
        Assert.True(updated.UpdatedAt >= beforeUpdatedAt);
    }

    [Fact]
    public void Delete_ShouldDeactivateExistingProduct_AndReturnExpectedFlag()
    {
        var repo = new InMemoryProductRepository();
        var created = repo.Add(new Product { Name = "ToDelete", Category = "Home", Price = 2m, Sku = "SKU-DEL", StockQuantity = 1 });

        var deleted = repo.Delete(created.Id);
        var missingDelete = repo.Delete(Guid.NewGuid());

        Assert.True(deleted);
        Assert.False(missingDelete);
        Assert.False(repo.GetById(created.Id)!.IsActive);
    }

    [Fact]
    public void TryDecrementStock_ShouldHandleMissingInsufficientAndSuccess()
    {
        var repo = new InMemoryProductRepository();
        var created = repo.Add(new Product { Name = "Stock", Category = "Home", Price = 15m, Sku = "SKU-STK", StockQuantity = 5 });

        var missing = repo.TryDecrementStock(Guid.NewGuid(), 1);
        var insufficient = repo.TryDecrementStock(created.Id, 10);
        var success = repo.TryDecrementStock(created.Id, 3);

        Assert.False(missing.Success);
        Assert.Equal(0, missing.RemainingStock);
        Assert.False(insufficient.Success);
        Assert.Equal(5, insufficient.RemainingStock);
        Assert.True(success.Success);
        Assert.Equal(2, success.RemainingStock);
    }
}