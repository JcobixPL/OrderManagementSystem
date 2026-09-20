namespace OrderManagement.Modules.Inventory.Application.Exceptions;

public sealed class InventoryItemNotFoundException : Exception
{
    public InventoryItemNotFoundException(Guid productId)
        : base($"Inventory item for product '{productId}' was not found.")
    {
        ProductId = productId;
    }

    public Guid ProductId { get; }
}