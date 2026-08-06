namespace MyEcomm.Contracts.Orders;

public class CheckoutRequest
{
    public string GuestId { get; set; } = string.Empty;
    public ShippingAddressDto ShippingAddress { get; set; } = new();
}
