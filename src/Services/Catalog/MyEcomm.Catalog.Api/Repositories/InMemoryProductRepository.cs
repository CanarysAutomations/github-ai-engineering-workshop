using System.Collections.Concurrent;
using MyEcomm.Catalog.Api.Models;

namespace MyEcomm.Catalog.Api.Repositories;

public class InMemoryProductRepository : IProductRepository
{
    private readonly ConcurrentDictionary<Guid, Product> _products = new();

    public void Seed(IEnumerable<Product> products)
    {
        foreach (var product in products)
        {
            _products[product.Id] = product;
        }
    }

    public (List<Product> Items, int TotalCount) GetAll(string? category, string? search, int page, int pageSize)
    {
        var query = _products.Values.Where(p => p.IsActive);

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(p => string.Equals(p.Category, category, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        var all = query.OrderBy(p => p.Name).ToList();
        var totalCount = all.Count;

        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 20 : pageSize;

        var items = all.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return (items, totalCount);
    }

    public Product? GetById(Guid id)
    {
        return _products.TryGetValue(id, out var product) ? product : null;
    }

    public Product Add(Product product)
    {
        product.Id = Guid.NewGuid();
        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;
        product.IsActive = true;
        _products[product.Id] = product;
        return product;
    }

    public Product? Update(Guid id, Product updated)
    {
        if (!_products.TryGetValue(id, out var existing))
        {
            return null;
        }

        lock (existing)
        {
            existing.Name = updated.Name;
            existing.Description = updated.Description;
            existing.Category = updated.Category;
            existing.Price = updated.Price;
            existing.Sku = updated.Sku;
            existing.StockQuantity = updated.StockQuantity;
            existing.ImageUrl = updated.ImageUrl;
            existing.IsActive = updated.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;
        }

        return existing;
    }

    public bool Delete(Guid id)
    {
        if (!_products.TryGetValue(id, out var existing))
        {
            return false;
        }

        lock (existing)
        {
            existing.IsActive = false;
            existing.UpdatedAt = DateTime.UtcNow;
        }

        return true;
    }

    public (bool Success, int RemainingStock) TryDecrementStock(Guid id, int quantity)
    {
        if (!_products.TryGetValue(id, out var existing))
        {
            return (false, 0);
        }

        lock (existing)
        {
            if (existing.StockQuantity < quantity)
            {
                return (false, existing.StockQuantity);
            }

            existing.StockQuantity -= quantity;
            existing.UpdatedAt = DateTime.UtcNow;
            return (true, existing.StockQuantity);
        }
    }
}
