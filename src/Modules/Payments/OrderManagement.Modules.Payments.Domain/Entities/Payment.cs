using OrderManagement.Modules.Payments.Domain.Enums;
using OrderManagement.Modules.Payments.Domain.Exceptions;

namespace OrderManagement.Modules.Payments.Domain.Entities;

public sealed class Payment
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public string Currency { get; private set; } = null!;
    public PaymentStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? PaidAt { get; private set; }

    private Payment() { }

    public Payment(
        Guid orderId,
        decimal amount,
        string currency)
    {
        if (orderId == Guid.Empty)
            throw new ArgumentException("Order ID cannot be empty.", nameof(orderId));

        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency is required.", nameof(currency));

        Id = Guid.NewGuid();
        OrderId = orderId;
        Amount = amount;
        Currency = currency;
        Status = PaymentStatus.Pending;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void Complete()
    {
        if (Status != PaymentStatus.Pending)
        {
            throw new InvalidPaymentStatusTransitionException(
                Status,
                PaymentStatus.Completed);
        }

        Status = PaymentStatus.Completed;
        PaidAt = DateTimeOffset.UtcNow;
    }

    public void Fail()
    {
        if (Status != PaymentStatus.Pending)
        {
            throw new InvalidPaymentStatusTransitionException(
                Status,
                PaymentStatus.Failed);
        }

        Status = PaymentStatus.Failed;
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
