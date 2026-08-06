using MyEcomm.Order.Api.Models;
using MyEcomm.Order.Api.Repositories;

namespace MyEcomm.Backend.Tests;

public class OrderRepositoryAndModelsTests
{
    [Fact]
    public void Add_ShouldAssignIdAndTimestamp()
    {
        var repo = new InMemoryOrderRepository();

        var added = repo.Add(new OrderRecord { UserId = "u1" });

        Assert.NotEqual(Guid.Empty, added.Id);
        Assert.True(added.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void GetByUserId_ShouldFilterAndSortDescending()
    {
        var repo = new InMemoryOrderRepository();
        var o1 = repo.Add(new OrderRecord { UserId = "u1" });
        var o2 = repo.Add(new OrderRecord { UserId = "u1" });
        repo.Add(new OrderRecord { UserId = "u2" });

        o1.CreatedAt = DateTime.UtcNow.AddMinutes(-1);
        o2.CreatedAt = DateTime.UtcNow;

        var orders = repo.GetByUserId("u1");

        Assert.Equal(2, orders.Count);
        Assert.Equal(o2.Id, orders[0].Id);
        Assert.Equal(o1.Id, orders[1].Id);
    }

    [Fact]
    public void GetById_ShouldHonorUserBoundary()
    {
        var repo = new InMemoryOrderRepository();
        var order = repo.Add(new OrderRecord { UserId = "u1" });

        var mine = repo.GetById(order.Id, "u1");
        var others = repo.GetById(order.Id, "u2");

        Assert.NotNull(mine);
        Assert.Null(others);
    }

    [Fact]
    public void OrderItemRecord_LineTotal_ShouldMultiplyPriceAndQuantity()
    {
        var item = new OrderItemRecord { UnitPrice = 12.5m, Quantity = 4 };

        Assert.Equal(50m, item.LineTotal);
    }
}