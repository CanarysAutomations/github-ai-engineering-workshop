namespace MyEcomm.Cart.Api.Models;

public class ShoppingCart
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string GuestId { get; set; } = string.Empty;
    public List<CartItem> Items { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
