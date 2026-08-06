using System.Collections.Concurrent;
using MyEcomm.Order.Api.Models;

namespace MyEcomm.Order.Api.Repositories;

public class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<Guid, OrderRecord> _orders = new();

    public OrderRecord Add(OrderRecord order)
    {
        order.Id = Guid.NewGuid();
        order.CreatedAt = DateTime.UtcNow;
        _orders[order.Id] = order;
        return order;
    }

    public List<OrderRecord> GetByUserId(string userId)
    {
        return _orders.Values
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .ToList();
    }

    public OrderRecord? GetById(Guid orderId, string userId)
    {
        return _orders.TryGetValue(orderId, out var order) && order.UserId == userId ? order : null;
    }
}
