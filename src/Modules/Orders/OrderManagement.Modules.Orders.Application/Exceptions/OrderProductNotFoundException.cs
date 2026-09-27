namespace OrderManagement.Modules.Orders.Application.Exceptions;

public sealed class OrderProductNotFoundException : Exception
{
    public OrderProductNotFoundException(Guid productId)
        : base($"Product '{productId}' was not found.")
    {
        ProductId = productId;
    }

    public Guid ProductId { get; }
}