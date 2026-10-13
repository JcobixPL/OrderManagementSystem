using Moq;
using OrderManagement.Modules.Payments.Application.Abstractions;
using OrderManagement.Modules.Payments.Application.Exceptions;
using OrderManagement.Modules.Payments.Application.Features.Payments.CompleteAttempt;
using OrderManagement.Modules.Payments.Domain.Entities;
using OrderManagement.Modules.Payments.Domain.Enums;
using OrderManagement.Modules.Payments.Domain.Exceptions;

namespace OrderManagement.Modules.Payments.UnitTests;

public sealed class CompletePaymentAttemptCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCompleteAttemptAndPaymentAndMarkOrderAsPaid_WhenDataIsValid()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var orderId = Guid.NewGuid();

        var payment = new Payment(
            orderId,
            customerId,
            100m,
            "PLN");

        var attempt = payment.CreateAttempt();

        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var unitOfWorkMock = new Mock<IPaymentsUnitOfWork>();
        var orderPaymentStatusUpdaterMock = new Mock<IOrderPaymentStatusUpdater>();
        var currentUserMock = new Mock<IPaymentCurrentUser>();

        paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                payment.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(payment);

        currentUserMock
            .SetupGet(x => x.UserId)
            .Returns(customerId);

        var handler = new CompletePaymentAttemptCommandHandler(
            paymentRepositoryMock.Object,
            unitOfWorkMock.Object,
            orderPaymentStatusUpdaterMock.Object,
            currentUserMock.Object);

        var command = new CompletePaymentAttemptCommand(
            payment.Id,
            attempt.Id,
            "PAY-123");

        // Act
        await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.Equal(PaymentAttemptStatus.Completed, attempt.Status);
        Assert.Equal("PAY-123", attempt.ExternalPaymentId);
        Assert.NotNull(attempt.CompletedAt);

        Assert.Equal(PaymentStatus.Completed, payment.Status);
        Assert.NotNull(payment.PaidAt);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);

        orderPaymentStatusUpdaterMock.Verify(
            x => x.MarkAsPaidAsync(
                orderId,
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowPaymentNotFoundException_WhenPaymentDoesNotExist()
    {
        // Arrange
        var paymentId = Guid.NewGuid();

        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var unitOfWorkMock = new Mock<IPaymentsUnitOfWork>();
        var orderPaymentStatusUpdaterMock = new Mock<IOrderPaymentStatusUpdater>();
        var currentUserMock = new Mock<IPaymentCurrentUser>();

        paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                paymentId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Payment?)null);

        var handler = new CompletePaymentAttemptCommandHandler(
            paymentRepositoryMock.Object,
            unitOfWorkMock.Object,
            orderPaymentStatusUpdaterMock.Object,
            currentUserMock.Object);

        var command = new CompletePaymentAttemptCommand(
            paymentId,
            Guid.NewGuid(),
            "PAY-123");

        // Act & Assert
        await Assert.ThrowsAsync<PaymentNotFoundException>(
            () => handler.Handle(
                command,
                CancellationToken.None));

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);

        orderPaymentStatusUpdaterMock.Verify(
            x => x.MarkAsPaidAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowPaymentAccessDeniedException_WhenPaymentBelongsToAnotherUser()
    {
        // Arrange
        var payment = new Payment(
            Guid.NewGuid(),
            Guid.NewGuid(),
            100m,
            "PLN");

        var attempt = payment.CreateAttempt();

        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var unitOfWorkMock = new Mock<IPaymentsUnitOfWork>();
        var orderPaymentStatusUpdaterMock = new Mock<IOrderPaymentStatusUpdater>();
        var currentUserMock = new Mock<IPaymentCurrentUser>();

        paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                payment.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(payment);

        currentUserMock
            .SetupGet(x => x.UserId)
            .Returns(Guid.NewGuid());

        var handler = new CompletePaymentAttemptCommandHandler(
            paymentRepositoryMock.Object,
            unitOfWorkMock.Object,
            orderPaymentStatusUpdaterMock.Object,
            currentUserMock.Object);

        var command = new CompletePaymentAttemptCommand(
            payment.Id,
            attempt.Id,
            "PAY-123");

        // Act & Assert
        await Assert.ThrowsAsync<PaymentAccessDeniedException>(
            () => handler.Handle(
                command,
                CancellationToken.None));

        Assert.Equal(PaymentAttemptStatus.Pending, attempt.Status);
        Assert.Equal(PaymentStatus.Pending, payment.Status);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);

        orderPaymentStatusUpdaterMock.Verify(
            x => x.MarkAsPaidAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowPaymentAttemptNotFoundException_WhenAttemptDoesNotExist()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        var payment = new Payment(
            Guid.NewGuid(),
            customerId,
            100m,
            "PLN");

        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var unitOfWorkMock = new Mock<IPaymentsUnitOfWork>();
        var orderPaymentStatusUpdaterMock = new Mock<IOrderPaymentStatusUpdater>();
        var currentUserMock = new Mock<IPaymentCurrentUser>();

        paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                payment.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(payment);

        currentUserMock
            .SetupGet(x => x.UserId)
            .Returns(customerId);

        var handler = new CompletePaymentAttemptCommandHandler(
            paymentRepositoryMock.Object,
            unitOfWorkMock.Object,
            orderPaymentStatusUpdaterMock.Object,
            currentUserMock.Object);

        var command = new CompletePaymentAttemptCommand(
            payment.Id,
            Guid.NewGuid(),
            "PAY-123");

        // Act & Assert
        await Assert.ThrowsAsync<PaymentAttemptNotFoundException>(
            () => handler.Handle(
                command,
                CancellationToken.None));

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);

        orderPaymentStatusUpdaterMock.Verify(
            x => x.MarkAsPaidAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidPaymentAttemptStatusTransitionException_WhenAttemptIsAlreadyFailed()
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
        var unitOfWorkMock = new Mock<IPaymentsUnitOfWork>();
        var orderPaymentStatusUpdaterMock = new Mock<IOrderPaymentStatusUpdater>();
        var currentUserMock = new Mock<IPaymentCurrentUser>();

        paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                payment.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(payment);

        currentUserMock
            .SetupGet(x => x.UserId)
            .Returns(customerId);

        var handler = new CompletePaymentAttemptCommandHandler(
            paymentRepositoryMock.Object,
            unitOfWorkMock.Object,
            orderPaymentStatusUpdaterMock.Object,
            currentUserMock.Object);

        var command = new CompletePaymentAttemptCommand(
            payment.Id,
            attempt.Id,
            "PAY-123");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidPaymentAttemptStatusTransitionException>(
            () => handler.Handle(
                command,
                CancellationToken.None));

        Assert.Equal(PaymentStatus.Pending, payment.Status);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);

        orderPaymentStatusUpdaterMock.Verify(
            x => x.MarkAsPaidAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}