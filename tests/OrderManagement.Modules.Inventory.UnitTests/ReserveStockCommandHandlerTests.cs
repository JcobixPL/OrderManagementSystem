using Moq;
using OrderManagement.Modules.Inventory.Application.Abstractions;
using OrderManagement.Modules.Inventory.Application.Exceptions;
using OrderManagement.Modules.Inventory.Application.Features.Inventory.Reserve;
using OrderManagement.Modules.Inventory.Domain.Entities;
using OrderManagement.Modules.Inventory.Domain.Exceptions;

namespace OrderManagement.Modules.Inventory.UnitTests;

public sealed class ReserveStockCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenInventoryExists_ShouldReserveStockAndSaveChanges()
    {
        var productId = Guid.NewGuid();
        var inventoryItem = new InventoryItem(productId, 10);

        var repositoryMock = new Mock<IInventoryRepository>();
        var unitOfWorkMock = new Mock<IInventoryUnitOfWork>();

        repositoryMock
            .Setup(x => x.GetByProductIdAsync(
                productId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(inventoryItem);

        var handler = new ReserveStockCommandHandler(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new ReserveStockCommand(
            productId, 3);

        await handler.Handle(
            command,
            CancellationToken.None);

        Assert.Equal(7, inventoryItem.AvailableQuantity);
        Assert.Equal(3, inventoryItem.ReservedQuantity);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenInventoryDoesNotExist_ShouldThrowInventoryItemNotFoundException()
    {
        var productId = Guid.NewGuid();

        var repositoryMock = new Mock<IInventoryRepository>();
        var unitOfWorkMock = new Mock<IInventoryUnitOfWork>();

        repositoryMock
            .Setup(x => x.GetByProductIdAsync(
                productId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((InventoryItem?)null);

        var handler = new ReserveStockCommandHandler(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new ReserveStockCommand(
            productId,
            3);

        await Assert.ThrowsAsync<InventoryItemNotFoundException>(
            () => handler.Handle(
                command,
                CancellationToken.None));

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_WhenStockIsInsufficient_ShouldThrowInsufficientStockException()
    {
        var productId = Guid.NewGuid();
        var inventoryItem = new InventoryItem(
            productId,
            2);

        var repositoryMock = new Mock<IInventoryRepository>();
        var unitOfWorkMock = new Mock<IInventoryUnitOfWork>();

        repositoryMock
            .Setup(x => x.GetByProductIdAsync(
                productId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(inventoryItem);

        var handler = new ReserveStockCommandHandler(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new ReserveStockCommand(
            productId,
            3);

        await Assert.ThrowsAsync<InsufficientStockException>(
            () => handler.Handle(
                command,
                CancellationToken.None));

        Assert.Equal(2, inventoryItem.AvailableQuantity);
        Assert.Equal(0, inventoryItem.ReservedQuantity);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
