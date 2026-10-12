using OrderManagement.Modules.Payments.Domain.Entities;
using OrderManagement.Modules.Payments.Domain.Enums;
using OrderManagement.Modules.Payments.Domain.Exceptions;

namespace OrderManagement.Modules.Payments.UnitTests;

public sealed class PaymentTests
{
    [Fact]
    public void Constructor_ShouldCreatePendingPayment_WhenDataIsValid()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        // Act
        var payment = new Payment(
            orderId,
            100.00m,
            "PLN");

        // Assert
        Assert.NotEqual(Guid.Empty, payment.Id);
        Assert.Equal(orderId, payment.OrderId);
        Assert.Equal(100.00m, payment.Amount);
        Assert.Equal("PLN", payment.Currency);
        Assert.Equal(PaymentStatus.Pending, payment.Status);
        Assert.Null(payment.PaidAt);
    }

    [Fact]
    public void Complete_ShouldSetStatusToCompletedAndSetPaidAt()
    {
        // Arrange
        var payment = new Payment(
            Guid.NewGuid(),
            100.00m,
            "PLN");

        // Act
        payment.Complete();

        // Assert
        Assert.Equal(PaymentStatus.Completed, payment.Status);
        Assert.NotNull(payment.PaidAt);
    }

    [Fact]
    public void Fail_ShouldSetStatusToFailed()
    {
        // Arrange
        var payment = new Payment(
            Guid.NewGuid(),
            100m,
            "PLN");

        // Act
        payment.Fail();

        // Assert
        Assert.Equal(PaymentStatus.Failed, payment.Status);
        Assert.Null(payment.PaidAt);
    }

    [Fact]
    public void Cancel_ShouldSetStatusToCancelled()
    {
        // Arrange
        var payment = new Payment(
            Guid.NewGuid(),
            100m,
            "PLN");

        // Act
        payment.Cancel();

        // Assert
        Assert.Equal(PaymentStatus.Cancelled, payment.Status);
        Assert.Null(payment.PaidAt);
    }

    [Fact]
    public void Complete_ShouldThrow_WhenPaymentIsNotPending()
    {
        // Arrange
        var payment = new Payment(
            Guid.NewGuid(),
            100m,
            "PLN");

        payment.Fail();

        // Act & Assert
        Assert.Throws<InvalidPaymentStatusTransitionException>(
            () => payment.Complete());
    }
}
