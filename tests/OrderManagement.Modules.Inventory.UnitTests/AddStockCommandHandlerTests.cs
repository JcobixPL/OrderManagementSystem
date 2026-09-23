using Moq;
using OrderManagement.Modules.Inventory.Application.Abstractions;
using OrderManagement.Modules.Inventory.Application.Features.Inventory.AddStock;
using OrderManagement.Modules.Inventory.Domain.Entities;

namespace OrderManagement.Modules.Inventory.UnitTests;

public sealed class AddStockCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenInventoryExists_ShouldAddStockAndSaveChanges()
    {
        var productId = Guid.NewGuid();
        var inventoryItem = new InventoryItem(productId, 5);

        var repositoryMock = new Mock<IInventoryRepository>();
        var unitOfWorkMock = new Mock<IInventoryUnitOfWork>();

        repositoryMock
            .Setup(x => x.GetByProductIdAsync(
                productId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(inventoryItem);

        var handler = new AddStockCommandHandler(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new AddStockCommand(
            productId,
            3);

        await handler.Handle(
            command,
            CancellationToken.None);

        Assert.Equal(8, inventoryItem.AvailableQuantity);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
