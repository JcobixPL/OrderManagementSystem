namespace OrderManagement.Modules.Orders.Application.Exceptions;

public sealed class InactiveOrderProductException : Exception
{
    public InactiveOrderProductException(Guid productId)
        : base($"Product '{productId}' is inactive.")
    {
        ProductId = productId;
    }

    public Guid ProductId { get; }
}