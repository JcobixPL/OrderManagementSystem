namespace OrderManagement.Modules.Payments.Application.Exceptions;

public sealed class PaymentAccessDeniedException : Exception
{
    public PaymentAccessDeniedException()
        : base("You do not have access to this payment.")
    {
    }
}