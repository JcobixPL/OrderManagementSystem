namespace OrderManagement.Modules.Inventory.Domain.Exceptions;

public sealed class InsufficientReservedStockException : Exception
{
    public InsufficientReservedStockException(
        int requestedQuantity,
        int reservedQuantity)
        : base(
            $"Cannot process {requestedQuantity} reserved item(s). " +
            $"Only {reservedQuantity} item(s) are currently reserved.")
    {
        RequestedQuantity = requestedQuantity;
        ReservedQuantity = reservedQuantity;
    }

    public int RequestedQuantity { get; }

    public int ReservedQuantity { get; }
}