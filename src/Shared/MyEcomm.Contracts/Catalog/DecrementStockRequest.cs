namespace MyEcomm.Contracts.Catalog;

public class DecrementStockRequest
{
    public int Quantity { get; set; }
}

public class DecrementStockResponse
{
    public bool Success { get; set; }
    public int RemainingStock { get; set; }
}
