using Moq;
using OrderManagement.Modules.Payments.Application.Abstractions;
using OrderManagement.Modules.Payments.Application.Exceptions;
using OrderManagement.Modules.Payments.Application.Features.Payments.GetById;
using OrderManagement.Modules.Payments.Domain.Entities;

namespace OrderManagement.Modules.Payments.UnitTests;

public sealed class GetPaymentByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnPayment_WhenPaymentBelongsToCurrentUser()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        var payment = new Payment(
            Guid.NewGuid(),
            customerId,
            100m,
            "PLN");

        var attempt = payment.CreateAttempt();
        attempt.Fail("Card declined");

        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var currentUserMock = new Mock<IPaymentCurrentUser>();

        paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                payment.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(payment);

        currentUserMock
            .SetupGet(x => x.UserId)
            .Returns(customerId);

        var handler = new GetPaymentByIdQueryHandler(
            paymentRepositoryMock.Object,
            currentUserMock.Object);

        // Act
        var result = await handler.Handle(
            new GetPaymentByIdQuery(payment.Id),
            CancellationToken.None);

        // Assert
        Assert.Equal(payment.Id, result.Id);
        Assert.Equal(payment.OrderId, result.OrderId);
        Assert.Equal(100m, result.Amount);
        Assert.Equal("PLN", result.Currency);
        Assert.Equal("Pending", result.Status);
        Assert.Single(result.Attempts);
        Assert.Equal("Failed", result.Attempts.Single().Status);
    }

    [Fact]
    public async Task Handle_ShouldThrowPaymentNotFoundException_WhenPaymentDoesNotExist()
    {
        var paymentId = Guid.NewGuid();

        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var currentUserMock = new Mock<IPaymentCurrentUser>();

        paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                paymentId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Payment?)null);

        var handler = new GetPaymentByIdQueryHandler(
            paymentRepositoryMock.Object,
            currentUserMock.Object);

        await Assert.ThrowsAsync<PaymentNotFoundException>(
            () => handler.Handle(
                new GetPaymentByIdQuery(paymentId),
                CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ShouldThrowPaymentAccessDeniedException_WhenPaymentBelongsToAnotherUser()
    {
        var payment = new Payment(
            Guid.NewGuid(),
            Guid.NewGuid(),
            100m,
            "PLN");

        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var currentUserMock = new Mock<IPaymentCurrentUser>();

        paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                payment.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(payment);

        currentUserMock
            .SetupGet(x => x.UserId)
            .Returns(Guid.NewGuid());

        var handler = new GetPaymentByIdQueryHandler(
            paymentRepositoryMock.Object,
            currentUserMock.Object);

        await Assert.ThrowsAsync<PaymentAccessDeniedException>(
            () => handler.Handle(
                new GetPaymentByIdQuery(payment.Id),
                CancellationToken.None));
    }
}