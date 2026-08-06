using MyEcomm.Cart.Api.Models;

namespace MyEcomm.Cart.Api.Repositories;

public interface ICartRepository
{
    ShoppingCart GetOrCreate(string guestId);
    ShoppingCart? Get(string guestId);
    void Clear(string guestId);
    void Save(ShoppingCart cart);
}
