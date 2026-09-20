namespace OrderManagement.Modules.Inventory.Application.Exceptions;

public sealed class InventoryItemAlreadyExistsException : Exception
{
    public InventoryItemAlreadyExistsException(Guid productId)
        : base($"Inventory item for product '{productId}' already exists.")
    {
        ProductId = productId;
    }
    
    public Guid ProductId { get; }
}
