using MyEcomm.Catalog.Api.Models;
using MyEcomm.Catalog.Api.Repositories;

namespace MyEcomm.Catalog.Api.Seed;

public static class ProductSeeder
{
    public static void SeedProducts(IProductRepository repository)
    {
        var now = DateTime.UtcNow;

        var products = new List<Product>
        {
            Make("Wireless Headphones", "Over-ear Bluetooth headphones with noise cancellation.", "Electronics", 79.99m, "ELEC-001", 25, now),
            Make("Mechanical Keyboard", "RGB backlit mechanical keyboard with blue switches.", "Electronics", 59.99m, "ELEC-002", 15, now),
            Make("4K Action Camera", "Waterproof action camera with image stabilization.", "Electronics", 129.99m, "ELEC-003", 0, now),
            Make("Smart Watch", "Fitness tracking smart watch with heart rate monitor.", "Electronics", 149.99m, "ELEC-004", 10, now),
            Make("Men's Denim Jacket", "Classic fit denim jacket.", "Apparel", 49.99m, "APRL-001", 30, now),
            Make("Women's Running Shoes", "Lightweight running shoes with cushioned sole.", "Apparel", 69.99m, "APRL-002", 20, now),
            Make("Cotton T-Shirt (3-Pack)", "Soft cotton crew-neck t-shirts, pack of 3.", "Apparel", 24.99m, "APRL-003", 50, now),
            Make("Ceramic Cookware Set", "10-piece non-stick ceramic cookware set.", "Home", 89.99m, "HOME-001", 12, now),
            Make("Memory Foam Pillow", "Contoured memory foam pillow for neck support.", "Home", 29.99m, "HOME-002", 40, now),
            Make("LED Desk Lamp", "Adjustable LED desk lamp with USB charging port.", "Home", 34.99m, "HOME-003", 18, now),
        };

        repository.Seed(products);
    }

    private static Product Make(string name, string description, string category, decimal price, string sku, int stock, DateTime now)
    {
        return new Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Category = category,
            Price = price,
            Sku = sku,
            StockQuantity = stock,
            ImageUrl = $"https://picsum.photos/seed/{sku}/400/300",
            IsActive = true,
            CreatedAt = now,
            UpdatedAt = now,
        };
    }
}
