using Moq;
using OrderManagement.Modules.Orders.Application.Abstractions;
using OrderManagement.Modules.Orders.Application.Features.Orders.MarkAsShipped;
using OrderManagement.Modules.Orders.Domain.Entities;
using OrderManagement.Modules.Orders.Domain.Enums;

namespace OrderManagement.Modules.Orders.UnitTests;

public sealed class MarkOrderAsShippedCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldMarkOrderAsShippedAndConfirmRemoval_WhenOrderIsReadyForShipment()
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

        order.MarkAsPaid();
        order.StartProcessing();
        order.MarkReadyForShipment();

        var orderRepositoryMock = new Mock<IOrderRepository>();
        var unitOfWorkMock = new Mock<IOrdersUnitOfWork>();
        var inventoryServiceMock = new Mock<IInventoryService>();

        orderRepositoryMock
            .Setup(x => x.GetByIdAsync(
                order.Id,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        var handler = new MarkOrderAsShippedCommandHandler(
            orderRepositoryMock.Object,
            unitOfWorkMock.Object,
            inventoryServiceMock.Object,
            new TestOrdersTransaction());

        var command = new MarkOrderAsShippedCommand(order.Id);

        // Act
        await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.Equal(OrderStatus.Shipped, order.Status);

        inventoryServiceMock.Verify(
            x => x.ConfirmRemovalAsync(
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