namespace OrderManagement.Modules.Payments.Application.Exceptions;

public sealed class PaymentNotFoundException : Exception
{
    public PaymentNotFoundException(Guid paymentId)
        : base($"Payment '{paymentId}' was not found.")
    {
    }
}
