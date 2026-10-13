using Moq;
using OrderManagement.Modules.Payments.Application.Abstractions;
using OrderManagement.Modules.Payments.Application.Exceptions;
using OrderManagement.Modules.Payments.Application.Features.Payments.Create;
using OrderManagement.Modules.Payments.Domain.Entities;

namespace OrderManagement.Modules.Payments.UnitTests;

public sealed class CreatePaymentCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreatePayment_WhenOrderIsPayableAndBelongsToCurrentUser()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();

        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var unitOfWorkMock = new Mock<IPaymentsUnitOfWork>();
        var orderPaymentDetailsProviderMock =
            new Mock<IOrderPaymentDetailsProvider>();
        var currentUserMock = new Mock<IPaymentCurrentUser>();

        paymentRepositoryMock
            .Setup(x => x.GetByOrderIdAsync(
                orderId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Payment?)null);

        orderPaymentDetailsProviderMock
            .Setup(x => x.GetAsync(
                orderId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OrderPaymentDetails(
                orderId,
                customerId,
                100.00m,
                "PLN",
                "AwaitingPayment"));

        currentUserMock
            .SetupGet(x => x.UserId)
            .Returns(customerId);

        var handler = new CreatePaymentCommandHandler(
            paymentRepositoryMock.Object,
            unitOfWorkMock.Object,
            orderPaymentDetailsProviderMock.Object,
            currentUserMock.Object);

        var command = new CreatePaymentCommand(orderId);

        // Act
        var paymentId = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, paymentId);

        paymentRepositoryMock.Verify(
            x => x.AddAsync(
                It.Is<Payment>(payment =>
                    payment.OrderId == orderId &&
                    payment.CustomerId == customerId &&
                    payment.Amount == 100.00m &&
                    payment.Currency == "PLN"),
                It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldThrowPaymentAlreadyExistsException_WhenPaymentAlreadyExists()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();

        var existingPayment = new Payment(
            orderId,
            customerId,
            100m,
            "PLN");

        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var unitOfWorkMock = new Mock<IPaymentsUnitOfWork>();
        var orderPaymentDetailsProviderMock =
            new Mock<IOrderPaymentDetailsProvider>();
        var currentUserMock = new Mock<IPaymentCurrentUser>();

        paymentRepositoryMock
            .Setup(x => x.GetByOrderIdAsync(
                orderId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingPayment);

        var handler = new CreatePaymentCommandHandler(
            paymentRepositoryMock.Object,
            unitOfWorkMock.Object,
            orderPaymentDetailsProviderMock.Object,
            currentUserMock.Object);

        var command = new CreatePaymentCommand(orderId);

        // Act & Assert
        await Assert.ThrowsAsync<PaymentAlreadyExistsException>(
            () => handler.Handle(
                command,
                CancellationToken.None));

        orderPaymentDetailsProviderMock.Verify(
            x => x.GetAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        paymentRepositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Payment>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowPaymentAccessDeniedException_WhenOrderBelongsToAnotherUser()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var orderCustomerId = Guid.NewGuid();
        var currentUserId = Guid.NewGuid();

        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var unitOfWorkMock = new Mock<IPaymentsUnitOfWork>();
        var orderPaymentDetailsProviderMock =
            new Mock<IOrderPaymentDetailsProvider>();
        var currentUserMock = new Mock<IPaymentCurrentUser>();

        paymentRepositoryMock
            .Setup(x => x.GetByOrderIdAsync(
                orderId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Payment?)null);

        orderPaymentDetailsProviderMock
            .Setup(x => x.GetAsync(
                orderId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OrderPaymentDetails(
                orderId,
                orderCustomerId,
                100m,
                "PLN",
                "AwaitingPayment"));

        currentUserMock
            .SetupGet(x => x.UserId)
            .Returns(currentUserId);

        var handler = new CreatePaymentCommandHandler(
            paymentRepositoryMock.Object,
            unitOfWorkMock.Object,
            orderPaymentDetailsProviderMock.Object,
            currentUserMock.Object);

        var command = new CreatePaymentCommand(orderId);

        // Act & Assert
        await Assert.ThrowsAsync<PaymentAccessDeniedException>(
            () => handler.Handle(
                command,
                CancellationToken.None));

        paymentRepositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Payment>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldThrowOrderNotPayableException_WhenOrderIsNotAwaitingPayment()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var customerId = Guid.NewGuid();

        var paymentRepositoryMock = new Mock<IPaymentRepository>();
        var unitOfWorkMock = new Mock<IPaymentsUnitOfWork>();
        var orderPaymentDetailsProviderMock =
            new Mock<IOrderPaymentDetailsProvider>();
        var currentUserMock = new Mock<IPaymentCurrentUser>();

        paymentRepositoryMock
            .Setup(x => x.GetByOrderIdAsync(
                orderId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Payment?)null);

        orderPaymentDetailsProviderMock
            .Setup(x => x.GetAsync(
                orderId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new OrderPaymentDetails(
                orderId,
                customerId,
                100m,
                "PLN",
                "Paid"));

        currentUserMock
            .SetupGet(x => x.UserId)
            .Returns(customerId);

        var handler = new CreatePaymentCommandHandler(
            paymentRepositoryMock.Object,
            unitOfWorkMock.Object,
            orderPaymentDetailsProviderMock.Object,
            currentUserMock.Object);

        var command = new CreatePaymentCommand(orderId);

        // Act & Assert
        await Assert.ThrowsAsync<OrderNotPayableException>(
            () => handler.Handle(
                command,
                CancellationToken.None));

        paymentRepositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Payment>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
