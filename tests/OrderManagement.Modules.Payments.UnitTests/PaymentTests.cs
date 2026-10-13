using OrderManagement.Modules.Payments.Domain.Entities;
using OrderManagement.Modules.Payments.Domain.Enums;
using OrderManagement.Modules.Payments.Domain.Exceptions;

namespace OrderManagement.Modules.Payments.UnitTests;

public sealed class PaymentTests
{
    [Fact]
    public void Constructor_ShouldCreatePendingPayment_WhenDataIsValid()
    {
        var orderId = Guid.NewGuid();

        var payment = new Payment(
            orderId,
            Guid.NewGuid(),
            199.99m,
            "pln");

        Assert.NotEqual(Guid.Empty, payment.Id);
        Assert.Equal(orderId, payment.OrderId);
        Assert.Equal(199.99m, payment.Amount);
        Assert.Equal("PLN", payment.Currency);
        Assert.Equal(PaymentStatus.Pending, payment.Status);
        Assert.Empty(payment.Attempts);
        Assert.Null(payment.PaidAt);
    }

    [Fact]
    public void CreateAttempt_ShouldAddPendingAttempt()
    {
        var payment = new Payment(
            Guid.NewGuid(),
            Guid.NewGuid(),
            100m,
            "PLN");

        var attempt = payment.CreateAttempt();

        Assert.Single(payment.Attempts);
        Assert.Equal(PaymentAttemptStatus.Pending, attempt.Status);
        Assert.Equal(payment.Id, attempt.PaymentId);
    }

    [Fact]
    public void CreateAttempt_ShouldThrow_WhenPendingAttemptAlreadyExists()
    {
        var payment = new Payment(
            Guid.NewGuid(),
            Guid.NewGuid(),
            100m,
            "PLN");

        payment.CreateAttempt();

        Assert.Throws<PendingPaymentAttemptAlreadyExistsException>(
            () => payment.CreateAttempt());
    }

    [Fact]
    public void CreateAttempt_ShouldAllowRetry_WhenPreviousAttemptFailed()
    {
        var payment = new Payment(
            Guid.NewGuid(),
            Guid.NewGuid(),
            100m,
            "PLN");

        var firstAttempt = payment.CreateAttempt();

        firstAttempt.Fail("Card declined");

        var secondAttempt = payment.CreateAttempt();

        Assert.Equal(2, payment.Attempts.Count);
        Assert.Equal(
            PaymentAttemptStatus.Failed,
            firstAttempt.Status);
        Assert.Equal(
            PaymentAttemptStatus.Pending,
            secondAttempt.Status);
    }

    [Fact]
    public void Complete_ShouldCompletePayment_WhenAttemptIsCompleted()
    {
        var payment = new Payment(
            Guid.NewGuid(),
            Guid.NewGuid(),
            100m,
            "PLN");

        var attempt = payment.CreateAttempt();

        attempt.Complete("external-payment-123");

        payment.Complete(attempt);

        Assert.Equal(
            PaymentStatus.Completed,
            payment.Status);

        Assert.NotNull(payment.PaidAt);
    }

    [Fact]
    public void Complete_ShouldThrow_WhenAttemptIsStillPending()
    {
        var payment = new Payment(
            Guid.NewGuid(),
            Guid.NewGuid(),
            100m,
            "PLN");

        var attempt = payment.CreateAttempt();

        Assert.Throws<InvalidOperationException>(
            () => payment.Complete(attempt));
    }

    [Fact]
    public void Cancel_ShouldSetStatusToCancelled()
    {
        var payment = new Payment(
            Guid.NewGuid(),
            Guid.NewGuid(),
            100m,
            "PLN");

        payment.Cancel();

        Assert.Equal(
            PaymentStatus.Cancelled,
            payment.Status);
    }

    [Fact]
    public void CreateAttempt_ShouldThrow_WhenPaymentIsCompleted()
    {
        var payment = new Payment(
            Guid.NewGuid(),
            Guid.NewGuid(),
            100m,
            "PLN");

        var attempt = payment.CreateAttempt();

        attempt.Complete("external-payment-123");

        payment.Complete(attempt);

        Assert.Throws<InvalidPaymentStatusTransitionException>(
            () => payment.CreateAttempt());
    }
}