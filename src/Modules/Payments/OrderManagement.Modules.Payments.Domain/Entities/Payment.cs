using OrderManagement.Modules.Payments.Domain.Enums;
using OrderManagement.Modules.Payments.Domain.Exceptions;

namespace OrderManagement.Modules.Payments.Domain.Entities;

public sealed class Payment
{
    private readonly List<PaymentAttempt> _attempts = [];

    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = null!;
    public PaymentStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? PaidAt { get; private set; }
    public IReadOnlyCollection<PaymentAttempt> Attempts => _attempts.AsReadOnly();

    private Payment() { }

    public Payment(
        Guid orderId,
        decimal amount,
        string currency)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentException(
                "Order ID cannot be empty.",
                nameof(orderId));

        if (amount <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(amount),
                "Amount must be greater than zero.");

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException(
                "Currency is required.",
                nameof(currency));

        Id = Guid.NewGuid();
        OrderId = orderId;
        Amount = amount;
        Currency = currency.Trim().ToUpperInvariant();
        Status = PaymentStatus.Pending;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public PaymentAttempt CreateAttempt()
    {
        if (Status != PaymentStatus.Pending)
        {
            throw new InvalidPaymentStatusTransitionException(
                Status,
                PaymentStatus.Pending);
        }

        var hasPendingAttempt = _attempts.Any(
            x => x.Status == PaymentAttemptStatus.Pending);

        if (hasPendingAttempt)
        {
            throw new InvalidOperationException(
                "Payment already has a pending attempt.");
        }

        var attempt = new PaymentAttempt(Id);

        _attempts.Add(attempt);

        return attempt;
    }

    public void Complete(PaymentAttempt attempt)
    {
        if (Status != PaymentStatus.Pending)
        {
            throw new InvalidPaymentStatusTransitionException(
                Status,
                PaymentStatus.Completed);
        }

        if (!_attempts.Contains(attempt))
        {
            throw new InvalidOperationException(
                "Payment attempt does not belong to this payment.");
        }

        if (attempt.Status != PaymentAttemptStatus.Completed)
        {
            throw new InvalidOperationException(
                "Payment attempt must be completed first.");
        }

        Status = PaymentStatus.Completed;
        PaidAt = DateTimeOffset.UtcNow;
    }

    public void Cancel()
    {
        if (Status != PaymentStatus.Pending)
        {
            throw new InvalidPaymentStatusTransitionException(
                Status,
                PaymentStatus.Cancelled);
        }

        Status = PaymentStatus.Cancelled;
    }
}
