namespace OrderManagement.Modules.Orders.Domain.Entities;

public sealed class OrderItem
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public string ProductSku { get; private set; } = null!;
    public string ProductName { get; private set; } = null!;
    public decimal UnitPrice { get; private set; }
    public string Currency { get; private set; } = null!;
    public int Quantity { get; private set; }
    public decimal TotalPrice => UnitPrice * Quantity;

    private OrderItem() { }

    public OrderItem(
        Guid productId,
        string productSku,
        string productName,
        decimal unitPrice,
        string currency,
        int quantity)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));

        if (string.IsNullOrWhiteSpace(productSku))
            throw new ArgumentException("Product SKU cannot be empty.", nameof(productSku));

        if (string.IsNullOrWhiteSpace(productName))
            throw new ArgumentException("Product name cannot be empty.", nameof(productName));

        if (unitPrice <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");

        Id = Guid.NewGuid();
        ProductId = productId;
        ProductSku = productSku;
        ProductName = productName;
        UnitPrice = unitPrice;
        Currency = currency;
        Quantity = quantity;
    }
}
