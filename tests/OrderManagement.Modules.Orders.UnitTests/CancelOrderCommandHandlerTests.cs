using Moq;
using OrderManagement.Modules.Orders.Application.Abstractions;
using OrderManagement.Modules.Orders.Application.Features.Orders.Cancel;
using OrderManagement.Modules.Orders.Domain.Entities;
using OrderManagement.Modules.Orders.Domain.Enums;

namespace OrderManagement.Modules.Orders.UnitTests;

public sealed class CancelOrderCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCancelOrderAndReleaseReservation_WhenOrderCanBeCancelled()
    {
        // Arrange
        var productId = Guid.NewGuid();

        var order = new Order(
            Guid.NewGuid(),
            "PLN");

        order.AddItem(
            productId,
            "SKU-001",
            "Test Product",
            100m,
            "PLN",
            2);

        var orderRepositoryMock = new Mock<IOrderRepository>();
        var unitOfWorkMock = new Mock<IOrdersUnitOfWork>();
        var inventoryServiceMock = new Mock<IInventoryService>();

        orderRepositoryMock
            .Setup(x => x.GetByIdAsync(
                order.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        var handler = new CancelOrderCommandHandler(
            orderRepositoryMock.Object,
            unitOfWorkMock.Object,
            inventoryServiceMock.Object,
            new TestOrdersTransaction());

        var command = new CancelOrderCommand(order.Id);

        // Act
        await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.Equal(OrderStatus.Cancelled, order.Status);

        inventoryServiceMock.Verify(
            x => x.ReleaseReservationAsync(
                productId,
                2,
                It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}