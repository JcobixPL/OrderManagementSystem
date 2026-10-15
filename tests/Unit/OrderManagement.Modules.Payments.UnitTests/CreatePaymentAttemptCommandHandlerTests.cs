using Moq;
using OrderManagement.Modules.Payments.Application.Abstractions;
using OrderManagement.Modules.Payments.Application.Exceptions;
using OrderManagement.Modules.Payments.Application.Features.Payments.CreateAttempt;
using OrderManagement.Modules.Payments.Domain.Entities;
using OrderManagement.Modules.Payments.Domain.Exceptions;

namespace OrderManagement.Modules.Payments.UnitTests;

public sealed class CreatePaymentAttemptCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateAttempt_WhenPaymentBelongsToCurrentUser()
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

        var handler = new CreatePaymentAttemptCommandHandler(
            paymentRepositoryMock.Object,
            unitOfWorkMock.Object,
            currentUserMock.Object);

        var command = new CreatePaymentAttemptCommand(payment.Id);

        // Act
        var attemptId = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, attemptId);
        Assert.Single(payment.Attempts);

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

        var handler = new CreatePaymentAttemptCommandHandler(
            paymentRepositoryMock.Object,
            unitOfWorkMock.Object,
            currentUserMock.Object);

        var command = new CreatePaymentAttemptCommand(paymentId);

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

        var handler = new CreatePaymentAttemptCommandHandler(
            paymentRepositoryMock.Object,
            unitOfWorkMock.Object,
            currentUserMock.Object);

        var command = new CreatePaymentAttemptCommand(payment.Id);

        // Act & Assert
        await Assert.ThrowsAsync<PaymentAccessDeniedException>(
            () => handler.Handle(
                command,
                CancellationToken.None));

        Assert.Empty(payment.Attempts);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowPendingPaymentAttemptAlreadyExistsException_WhenPendingAttemptExists()
    {
        // Arrange
        var customerId = Guid.NewGuid();

        var payment = new Payment(
            Guid.NewGuid(),
            customerId,
            100m,
            "PLN");

        payment.CreateAttempt();

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

        var handler = new CreatePaymentAttemptCommandHandler(
            paymentRepositoryMock.Object,
            unitOfWorkMock.Object,
            currentUserMock.Object);

        var command = new CreatePaymentAttemptCommand(payment.Id);

        // Act & Assert
        await Assert.ThrowsAsync<PendingPaymentAttemptAlreadyExistsException>(
            () => handler.Handle(
                command,
                CancellationToken.None));

        Assert.Single(payment.Attempts);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}