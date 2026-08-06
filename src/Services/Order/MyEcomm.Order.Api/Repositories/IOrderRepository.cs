using MyEcomm.Order.Api.Models;

namespace MyEcomm.Order.Api.Repositories;

public interface IOrderRepository
{
    OrderRecord Add(OrderRecord order);
    List<OrderRecord> GetByUserId(string userId);
    OrderRecord? GetById(Guid orderId, string userId);
}
