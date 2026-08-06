using MyEcomm.Cart.Api.Models;
using MyEcomm.Cart.Api.Repositories;

namespace MyEcomm.Backend.Tests;

public class CartRepositoryTests
{
    [Fact]
    public void GetOrCreate_ShouldCreateAndReturnSameCart()
    {
        var repo = new InMemoryCartRepository();

        var first = repo.GetOrCreate("guest-1");
        var second = repo.GetOrCreate("guest-1");

        Assert.Same(first, second);
        Assert.Equal("guest-1", first.GuestId);
    }

    [Fact]
    public void Get_ShouldReturnNull_WhenCartMissing()
    {
        var repo = new InMemoryCartRepository();

        var cart = repo.Get("missing");

        Assert.Null(cart);
    }

    [Fact]
    public void Save_ShouldPersistCart()
    {
        var repo = new InMemoryCartRepository();
        var cart = new ShoppingCart { GuestId = "guest-2" };

        repo.Save(cart);
        var loaded = repo.Get("guest-2");

        Assert.NotNull(loaded);
        Assert.Equal(cart.Id, loaded!.Id);
    }

    [Fact]
    public void Clear_ShouldRemoveItemsAndUpdateTimestamp()
    {
        var repo = new InMemoryCartRepository();
        var cart = repo.GetOrCreate("guest-3");
        cart.Items.Add(new CartItem { ProductName = "Keyboard", Quantity = 1, UnitPrice = 99m });
        var beforeUpdate = cart.UpdatedAt;

        repo.Clear("guest-3");

        Assert.Empty(cart.Items);
        Assert.True(cart.UpdatedAt >= beforeUpdate);
    }

    [Fact]
    public void Clear_ShouldNotThrowOrCreateCart_WhenCartMissing()
    {
        var repo = new InMemoryCartRepository();

        var exception = Record.Exception(() => repo.Clear("missing-guest"));
        var cart = repo.Get("missing-guest");

        Assert.Null(exception);
        Assert.Null(cart);
    }
}