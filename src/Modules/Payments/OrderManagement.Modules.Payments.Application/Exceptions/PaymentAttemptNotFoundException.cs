namespace OrderManagement.Modules.Payments.Application.Exceptions;

public sealed class PaymentAttemptNotFoundException : Exception
{
    public PaymentAttemptNotFoundException(Guid attemptId)
        : base($"Payment attempt '{attemptId}' was not found.")
    {
    }
}