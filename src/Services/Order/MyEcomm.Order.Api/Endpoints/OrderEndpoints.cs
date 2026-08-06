using System.Security.Claims;
using MyEcomm.Contracts.Common;
using MyEcomm.Contracts.Orders;
using MyEcomm.Order.Api.Clients;
using MyEcomm.Order.Api.Models;
using MyEcomm.Order.Api.Repositories;

namespace MyEcomm.Order.Api.Endpoints;

public static class OrderEndpoints
{
    private const decimal FlatShippingCost = 5.00m;

    public static void MapOrderEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/orders").RequireAuthorization();

        group.MapPost("/checkout", async (
            CheckoutRequest request,
            ClaimsPrincipal user,
            CartServiceClient cartClient,
            CatalogServiceClient catalogClient,
            IOrderRepository orderRepo) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            var cart = await cartClient.GetCartAsync(request.GuestId);
            if (cart is null || cart.Items.Count == 0)
            {
                return Results.BadRequest(new ErrorResponse { Message = "Cart is empty." });
            }

            var orderItems = cart.Items.Select(i => new OrderItemRecord
            {
                ProductId = i.ProductId,
                ProductName = i.ProductName,
                UnitPrice = i.UnitPrice,
                Quantity = i.Quantity,
            }).ToList();

            var subtotal = orderItems.Sum(i => i.LineTotal);

            var order = orderRepo.Add(new OrderRecord
            {
                UserId = userId,
                Status = OrderStatus.Placed,
                ShippingName = request.ShippingAddress.Name,
                ShippingAddress = request.ShippingAddress.Address,
                ShippingCity = request.ShippingAddress.City,
                ShippingZip = request.ShippingAddress.Zip,
                Items = orderItems,
                Subtotal = subtotal,
                ShippingCost = FlatShippingCost,
                TotalAmount = subtotal + FlatShippingCost,
            });

            foreach (var item in orderItems)
            {
                await catalogClient.DecrementStockAsync(item.ProductId, item.Quantity);
            }

            await cartClient.ClearCartAsync(request.GuestId);

            return Results.Created($"/api/orders/{order.Id}", ToDto(order));
        });

        group.MapGet("/", (ClaimsPrincipal user, IOrderRepository orderRepo) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            var orders = orderRepo.GetByUserId(userId).Select(ToDto).ToList();
            return Results.Ok(orders);
        });

        group.MapGet("/{orderId:guid}", (Guid orderId, ClaimsPrincipal user, IOrderRepository orderRepo) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");
            if (string.IsNullOrEmpty(userId))
            {
                return Results.Unauthorized();
            }

            var order = orderRepo.GetById(orderId, userId);
            return order is null ? Results.NotFound(new ErrorResponse { Message = "Order not found." }) : Results.Ok(ToDto(order));
        });
    }

    private static OrderDto ToDto(OrderRecord order) => new()
    {
        Id = order.Id,
        UserId = order.UserId,
        Status = order.Status.ToString(),
        ShippingAddress = new ShippingAddressDto
        {
            Name = order.ShippingName,
            Address = order.ShippingAddress,
            City = order.ShippingCity,
            Zip = order.ShippingZip,
        },
        Items = order.Items.Select(i => new OrderItemDto
        {
            Id = i.Id,
            ProductId = i.ProductId,
            ProductName = i.ProductName,
            UnitPrice = i.UnitPrice,
            Quantity = i.Quantity,
            LineTotal = i.LineTotal,
        }).ToList(),
        Subtotal = order.Subtotal,
        ShippingCost = order.ShippingCost,
        TotalAmount = order.TotalAmount,
        CreatedAt = order.CreatedAt,
    };
}
