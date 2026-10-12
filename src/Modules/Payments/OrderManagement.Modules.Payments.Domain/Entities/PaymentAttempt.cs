using OrderManagement.Modules.Payments.Domain.Enums;
using OrderManagement.Modules.Payments.Domain.Exceptions;

namespace OrderManagement.Modules.Payments.Domain.Entities;

public sealed class PaymentAttempt
{
    public Guid Id { get; private set; }

    public Guid PaymentId { get; private set; }

    public PaymentAttemptStatus Status { get; private set; }

    public string? ExternalPaymentId { get; private set; }

    public string? FailureReason { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset? CompletedAt { get; private set; }

    private PaymentAttempt()
    {
    }

    internal PaymentAttempt(Guid paymentId)
    {
        if (paymentId == Guid.Empty)
            throw new ArgumentException(
                "Payment ID cannot be empty.",
                nameof(paymentId));

        Id = Guid.NewGuid();
        PaymentId = paymentId;
        Status = PaymentAttemptStatus.Pending;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void Complete(string externalPaymentId)
    {
        if (Status != PaymentAttemptStatus.Pending)
        {
            throw new InvalidPaymentAttemptStatusTransitionException(
                Status,
                PaymentAttemptStatus.Completed);
        }

        if (string.IsNullOrWhiteSpace(externalPaymentId))
        {
            throw new ArgumentException(
                "External payment ID is required.",
                nameof(externalPaymentId));
        }

        Status = PaymentAttemptStatus.Completed;
        ExternalPaymentId = externalPaymentId.Trim();
        CompletedAt = DateTimeOffset.UtcNow;
    }

    public void Fail(string failureReason)
    {
        if (Status != PaymentAttemptStatus.Pending)
        {
            throw new InvalidPaymentAttemptStatusTransitionException(
                Status,
                PaymentAttemptStatus.Failed);
        }

        if (string.IsNullOrWhiteSpace(failureReason))
        {
            throw new ArgumentException(
                "Failure reason is required.",
                nameof(failureReason));
        }

        Status = PaymentAttemptStatus.Failed;
        FailureReason = failureReason.Trim();
        CompletedAt = DateTimeOffset.UtcNow;
    }
}