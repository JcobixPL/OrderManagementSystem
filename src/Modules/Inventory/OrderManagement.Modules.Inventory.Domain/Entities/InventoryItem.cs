using OrderManagement.Modules.Inventory.Domain.Exceptions;

namespace OrderManagement.Modules.Inventory.Domain.Entities;

public sealed class InventoryItem
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int ReservedQuantity { get; set; }
    public int AvailableQuantity { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    private InventoryItem() { }

    public InventoryItem(Guid productId, int initialQuantity)
    {
        if (productId == Guid.Empty)
            throw new ArgumentException("Product ID cannot be empty.", nameof(productId));

        if (initialQuantity < 0)
            throw new ArgumentOutOfRangeException(
                nameof(initialQuantity),
                "Initial quantity cannot be lower than zero.");

        Id = Guid.NewGuid();
        ProductId = productId;
        AvailableQuantity = initialQuantity;
        ReservedQuantity = 0;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                "Quantity must be greater than zero.");

        AvailableQuantity += quantity;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Reserve(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                "Quantity must be greater than zero.");

        if (quantity > AvailableQuantity)
            throw new InsufficientStockException(
                quantity, AvailableQuantity);

        AvailableQuantity -= quantity;
        ReservedQuantity += quantity;
        UpdatedAt = DateTimeOffset.Now;
    }

    public void ReleaseReservation(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                "Quantity must be greater than zero.");

        if (quantity > ReservedQuantity)
            throw new InsufficientReservedStockException(
                quantity, ReservedQuantity);

        ReservedQuantity -= quantity;
        AvailableQuantity += quantity;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void ConfirmRemoval(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                "Quantity must be greater than zero.");

        if (quantity > ReservedQuantity)
            throw new InsufficientReservedStockException(
                quantity, ReservedQuantity);

        ReservedQuantity -= quantity;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
