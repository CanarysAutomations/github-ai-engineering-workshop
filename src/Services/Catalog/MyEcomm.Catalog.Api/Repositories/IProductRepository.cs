using MyEcomm.Catalog.Api.Models;

namespace MyEcomm.Catalog.Api.Repositories;

public interface IProductRepository
{
    void Seed(IEnumerable<Product> products);
    (List<Product> Items, int TotalCount) GetAll(string? category, string? search, int page, int pageSize);
    Product? GetById(Guid id);
    Product Add(Product product);
    Product? Update(Guid id, Product updated);
    bool Delete(Guid id);
    (bool Success, int RemainingStock) TryDecrementStock(Guid id, int quantity);
}
