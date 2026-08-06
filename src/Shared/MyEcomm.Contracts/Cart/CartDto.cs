namespace MyEcomm.Contracts.Cart;

public class CartDto
{
    public Guid Id { get; set; }
    public string GuestId { get; set; } = string.Empty;
    public List<CartItemDto> Items { get; set; } = new();
    public decimal Total => Items.Sum(i => i.LineTotal);
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
