namespace OrderManagement.Modules.Inventory.Domain.Exceptions;

public sealed class InsufficientStockException : Exception
{
    public InsufficientStockException(int requestedQuantity, int availableQuantity)
        : base(
            $"Cannot reserve {requestedQuantity} item(s). " + 
            $"Only {availableQuantity} item(s) are available.")
    {
        RequestedQuantity = requestedQuantity;
        AvailableQuantity = availableQuantity;
    }

    public int RequestedQuantity { get; }
    public int AvailableQuantity { get; }
}
