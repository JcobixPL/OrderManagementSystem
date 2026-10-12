using OrderManagement.Modules.Payments.Domain.Enums;

namespace OrderManagement.Modules.Payments.Domain.Exceptions;

public sealed class InvalidPaymentAttemptStatusTransitionException : Exception
{
    public InvalidPaymentAttemptStatusTransitionException(
        PaymentAttemptStatus currentStatus,
        PaymentAttemptStatus targetStatus)
        : base(
            $"Payment attempt status cannot be changed from {currentStatus} to {targetStatus}.")
    {
    }
}