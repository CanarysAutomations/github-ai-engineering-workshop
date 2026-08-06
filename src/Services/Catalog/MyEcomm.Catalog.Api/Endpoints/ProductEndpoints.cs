using MyEcomm.Catalog.Api.Models;
using MyEcomm.Catalog.Api.Repositories;
using MyEcomm.Contracts.Catalog;
using MyEcomm.Contracts.Common;

namespace MyEcomm.Catalog.Api.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/catalog/products");

        group.MapPost("/", (CreateProductRequest request, IProductRepository repo) =>
        {
            var product = repo.Add(new Product
            {
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                Price = request.Price,
                Sku = request.Sku,
                StockQuantity = request.StockQuantity,
                ImageUrl = request.ImageUrl,
            });

            return Results.Created($"/api/catalog/products/{product.Id}", ToDto(product));
        });

        group.MapGet("/", (string? category, string? search, int? page, int? pageSize, IProductRepository repo) =>
        {
            var resolvedPage = page.GetValueOrDefault(1) < 1 ? 1 : page.GetValueOrDefault(1);
            var resolvedPageSize = pageSize.GetValueOrDefault(20) < 1 ? 20 : pageSize.GetValueOrDefault(20);
            var (items, totalCount) = repo.GetAll(category, search, resolvedPage, resolvedPageSize);
            return Results.Ok(new PagedResult<ProductDto>
            {
                Items = items.Select(ToDto).ToList(),
                Page = resolvedPage,
                PageSize = resolvedPageSize,
                TotalCount = totalCount,
            });
        });

        group.MapGet("/{id:guid}", (Guid id, IProductRepository repo) =>
        {
            var product = repo.GetById(id);
            return product is null ? Results.NotFound(new ErrorResponse { Message = "Product not found." }) : Results.Ok(ToDto(product));
        });

        group.MapPut("/{id:guid}", (Guid id, UpdateProductRequest request, IProductRepository repo) =>
        {
            var updated = repo.Update(id, new Product
            {
                Name = request.Name,
                Description = request.Description,
                Category = request.Category,
                Price = request.Price,
                Sku = request.Sku,
                StockQuantity = request.StockQuantity,
                ImageUrl = request.ImageUrl,
                IsActive = request.IsActive,
            });

            return updated is null ? Results.NotFound(new ErrorResponse { Message = "Product not found." }) : Results.Ok(ToDto(updated));
        });

        group.MapDelete("/{id:guid}", (Guid id, IProductRepository repo) =>
        {
            return repo.Delete(id) ? Results.NoContent() : Results.NotFound(new ErrorResponse { Message = "Product not found." });
        });

        group.MapPost("/{id:guid}/decrement-stock", (Guid id, DecrementStockRequest request, IProductRepository repo) =>
        {
            var (success, remainingStock) = repo.TryDecrementStock(id, request.Quantity);
            var response = new DecrementStockResponse { Success = success, RemainingStock = remainingStock };
            return success ? Results.Ok(response) : Results.Conflict(response);
        });
    }

    private static ProductDto ToDto(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        Category = product.Category,
        Price = product.Price,
        Sku = product.Sku,
        StockQuantity = product.StockQuantity,
        ImageUrl = product.ImageUrl,
        IsActive = product.IsActive,
        CreatedAt = product.CreatedAt,
        UpdatedAt = product.UpdatedAt,
    };
}
