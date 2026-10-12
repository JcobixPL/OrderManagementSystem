using OrderManagement.Modules.Payments.Domain.Enums;

namespace OrderManagement.Modules.Payments.Domain.Exceptions;

public sealed class InvalidPaymentStatusTransitionException : Exception
{
    public InvalidPaymentStatusTransitionException(
        PaymentStatus currentStatus,
        PaymentStatus targetStatus)
        : base(
            $"Payment status cannot be changed from {currentStatus} to {targetStatus}.")
    {
    }
}