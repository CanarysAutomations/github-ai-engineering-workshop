using MyEcomm.Cart.Api.Clients;
using MyEcomm.Cart.Api.Models;
using MyEcomm.Cart.Api.Repositories;
using MyEcomm.Contracts.Cart;
using MyEcomm.Contracts.Common;

namespace MyEcomm.Cart.Api.Endpoints;

public static class CartEndpoints
{
    public static void MapCartEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/cart");

        group.MapGet("/{guestId}", (string guestId, ICartRepository repo) =>
        {
            var cart = repo.GetOrCreate(guestId);
            return Results.Ok(ToDto(cart));
        });

        group.MapPost("/{guestId}/items", async (string guestId, AddCartItemRequest request, ICartRepository repo, CatalogServiceClient catalogClient) =>
        {
            var product = await catalogClient.GetProductAsync(request.ProductId);
            if (product is null)
            {
                return Results.NotFound(new ErrorResponse { Message = "Product not found." });
            }

            if (product.StockQuantity < request.Quantity)
            {
                return Results.Conflict(new ErrorResponse { Message = $"Only {product.StockQuantity} unit(s) of '{product.Name}' available." });
            }

            var cart = repo.GetOrCreate(guestId);
            lock (cart)
            {
                var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
                if (existingItem is not null)
                {
                    existingItem.Quantity += request.Quantity;
                    existingItem.UnitPrice = product.Price;
                    existingItem.ProductName = product.Name;
                }
                else
                {
                    cart.Items.Add(new CartItem
                    {
                        ProductId = product.Id,
                        ProductName = product.Name,
                        UnitPrice = product.Price,
                        Quantity = request.Quantity,
                    });
                }

                cart.UpdatedAt = DateTime.UtcNow;
            }

            return Results.Ok(ToDto(cart));
        });

        group.MapPut("/{guestId}/items/{itemId:guid}", (string guestId, Guid itemId, UpdateCartItemRequest request, ICartRepository repo) =>
        {
            var cart = repo.Get(guestId);
            if (cart is null)
            {
                return Results.NotFound(new ErrorResponse { Message = "Cart not found." });
            }

            lock (cart)
            {
                var item = cart.Items.FirstOrDefault(i => i.Id == itemId);
                if (item is null)
                {
                    return Results.NotFound(new ErrorResponse { Message = "Cart item not found." });
                }

                item.Quantity = request.Quantity;
                cart.UpdatedAt = DateTime.UtcNow;
            }

            return Results.Ok(ToDto(cart));
        });

        group.MapDelete("/{guestId}/items/{itemId:guid}", (string guestId, Guid itemId, ICartRepository repo) =>
        {
            var cart = repo.Get(guestId);
            if (cart is null)
            {
                return Results.NotFound(new ErrorResponse { Message = "Cart not found." });
            }

            lock (cart)
            {
                cart.Items.RemoveAll(i => i.Id == itemId);
                cart.UpdatedAt = DateTime.UtcNow;
            }

            return Results.Ok(ToDto(cart));
        });

        group.MapDelete("/{guestId}", (string guestId, ICartRepository repo) =>
        {
            repo.Clear(guestId);
            return Results.NoContent();
        });
    }

    private static CartDto ToDto(ShoppingCart cart) => new()
    {
        Id = cart.Id,
        GuestId = cart.GuestId,
        Items = cart.Items.Select(i => new CartItemDto
        {
            Id = i.Id,
            ProductId = i.ProductId,
            ProductName = i.ProductName,
            UnitPrice = i.UnitPrice,
            Quantity = i.Quantity,
        }).ToList(),
        CreatedAt = cart.CreatedAt,
        UpdatedAt = cart.UpdatedAt,
    };
}
