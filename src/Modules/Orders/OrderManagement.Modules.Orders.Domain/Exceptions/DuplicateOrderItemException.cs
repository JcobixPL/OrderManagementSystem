namespace OrderManagement.Modules.Orders.Domain.Exceptions;

public sealed class DuplicateOrderItemException : Exception
{
    public DuplicateOrderItemException(Guid productId)
        : base($"Product '{productId}' already exists in the order.")
    {
        ProductId = productId;
    }

    public Guid ProductId { get; }
}
