using Moq;
using OrderManagement.Modules.Payments.Application.Abstractions;
using OrderManagement.Modules.Payments.Application.Exceptions;
using OrderManagement.Modules.Payments.Application.Features.Payments.FailAttempt;
using OrderManagement.Modules.Payments.Domain.Entities;
using OrderManagement.Modules.Payments.Domain.Enums;
using OrderManagement.Modules.Payments.Domain.Exceptions;

namespace OrderManagement.Modules.Payments.UnitTests;

public sealed class FailPaymentAttemptCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldFailAttempt_WhenPaymentBelongsToCurrentUser()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        var payment = new Payment(
            Guid.NewGuid(),
            customerId,
            100m,
            "PLN");

        var attempt = payment.CreateAttempt();

        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var unitOfWorkMock = new Mock<IPaymentsUnitOfWork>();
        var currentUserMock = new Mock<IPaymentCurrentUser>();

        paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                payment.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(payment);

        currentUserMock
            .SetupGet(x => x.UserId)
            .Returns(customerId);

        var handler = new FailPaymentAttemptCommandHandler(
            paymentRepositoryMock.Object,
            unitOfWorkMock.Object,
            currentUserMock.Object);

        var command = new FailPaymentAttemptCommand(
            payment.Id,
            attempt.Id,
            "Card declined");

        // Act
        await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.Equal(PaymentAttemptStatus.Failed, attempt.Status);
        Assert.Equal("Card declined", attempt.FailureReason);
        Assert.NotNull(attempt.CompletedAt);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
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
        var currentUserMock = new Mock<IPaymentCurrentUser>();

        paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                paymentId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Payment?)null);

        var handler = new FailPaymentAttemptCommandHandler(
            paymentRepositoryMock.Object,
            unitOfWorkMock.Object,
            currentUserMock.Object);

        var command = new FailPaymentAttemptCommand(
            paymentId,
            Guid.NewGuid(),
            "Card declined");

        // Act & Assert
        await Assert.ThrowsAsync<PaymentNotFoundException>(
            () => handler.Handle(
                command,
                CancellationToken.None));

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
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
        var currentUserMock = new Mock<IPaymentCurrentUser>();

        paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                payment.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(payment);

        currentUserMock
            .SetupGet(x => x.UserId)
            .Returns(Guid.NewGuid());

        var handler = new FailPaymentAttemptCommandHandler(
            paymentRepositoryMock.Object,
            unitOfWorkMock.Object,
            currentUserMock.Object);

        var command = new FailPaymentAttemptCommand(
            payment.Id,
            attempt.Id,
            "Card declined");

        // Act & Assert
        await Assert.ThrowsAsync<PaymentAccessDeniedException>(
            () => handler.Handle(
                command,
                CancellationToken.None));

        Assert.Equal(PaymentAttemptStatus.Pending, attempt.Status);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
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
        var currentUserMock = new Mock<IPaymentCurrentUser>();

        paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                payment.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(payment);

        currentUserMock
            .SetupGet(x => x.UserId)
            .Returns(customerId);

        var handler = new FailPaymentAttemptCommandHandler(
            paymentRepositoryMock.Object,
            unitOfWorkMock.Object,
            currentUserMock.Object);

        var command = new FailPaymentAttemptCommand(
            payment.Id,
            Guid.NewGuid(),
            "Card declined");

        // Act & Assert
        await Assert.ThrowsAsync<PaymentAttemptNotFoundException>(
            () => handler.Handle(
                command,
                CancellationToken.None));

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
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
        attempt.Fail("First failure");

        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var unitOfWorkMock = new Mock<IPaymentsUnitOfWork>();
        var currentUserMock = new Mock<IPaymentCurrentUser>();

        paymentRepositoryMock
            .Setup(x => x.GetByIdAsync(
                payment.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(payment);

        currentUserMock
            .SetupGet(x => x.UserId)
            .Returns(customerId);

        var handler = new FailPaymentAttemptCommandHandler(
            paymentRepositoryMock.Object,
            unitOfWorkMock.Object,
            currentUserMock.Object);

        var command = new FailPaymentAttemptCommand(
            payment.Id,
            attempt.Id,
            "Second failure");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidPaymentAttemptStatusTransitionException>(
            () => handler.Handle(
                command,
                CancellationToken.None));

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}