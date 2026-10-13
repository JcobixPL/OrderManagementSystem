namespace OrderManagement.Modules.Payments.Domain.Exceptions;

public sealed class PendingPaymentAttemptAlreadyExistsException : Exception
{
    public PendingPaymentAttemptAlreadyExistsException()
        : base("Payment already has a pending attempt.")
    {
    }
}