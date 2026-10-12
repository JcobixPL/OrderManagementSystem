namespace OrderManagement.Modules.Payments.Application.Exceptions;

public sealed class PaymentAlreadyExistsException : Exception
{
    public PaymentAlreadyExistsException(Guid orderId)
        : base($"A payment for order '{orderId}' already exists.")
    {
    }
}