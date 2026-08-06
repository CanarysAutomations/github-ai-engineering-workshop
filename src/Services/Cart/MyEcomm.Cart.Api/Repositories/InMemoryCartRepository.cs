using System.Collections.Concurrent;
using MyEcomm.Cart.Api.Models;

namespace MyEcomm.Cart.Api.Repositories;

public class InMemoryCartRepository : ICartRepository
{
    private readonly ConcurrentDictionary<string, ShoppingCart> _carts = new();

    public ShoppingCart GetOrCreate(string guestId)
    {
        return _carts.GetOrAdd(guestId, id => new ShoppingCart { GuestId = id });
    }

    public ShoppingCart? Get(string guestId)
    {
        return _carts.TryGetValue(guestId, out var cart) ? cart : null;
    }

    public void Clear(string guestId)
    {
        if (_carts.TryGetValue(guestId, out var cart))
        {
            lock (cart)
            {
                cart.Items.Clear();
                cart.UpdatedAt = DateTime.UtcNow;
            }
        }
    }

    public void Save(ShoppingCart cart)
    {
        _carts[cart.GuestId] = cart;
    }
}
