namespace OrderManagement.Modules.Payments.Application.Exceptions;

public sealed class OrderNotPayableException : Exception
{
    public OrderNotPayableException(Guid orderId, string status)
        : base($"Order '{orderId}' cannot be paid when its status is '{status}'.")
    {
    }
}