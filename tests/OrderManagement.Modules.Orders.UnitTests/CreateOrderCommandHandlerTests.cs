using Moq;
using OrderManagement.Modules.Orders.Application.Abstractions;
using OrderManagement.Modules.Orders.Application.Features.Orders.Create;
using OrderManagement.Modules.Orders.Domain.Entities;

namespace OrderManagement.Modules.Orders.UnitTests;

public sealed class CreateOrderCommandHandlerTests
{
    [Fact]
    public async Task Handle_ShouldCreateOrderAndReserveStock_WhenRequestIsValid()
    {
        //Arrange
        var customerId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var product = new ProductSnapshot(
            productId,
            "SKU-001",
            "Test Product",
            99.99m,
            "PLN",
            true);

        var command = new CreateOrderCommand(
            customerId,
            new[]
            {
                new CreateOrderItem(productId, 2)
            });

        var productCatalogMock = new Mock<IProductCatalog>();
        var inventoryServiceMock = new Mock<IInventoryService>();
        var orderRepositoryMock = new Mock<IOrderRepository>();
        var unitOfWorkMock = new Mock<IOrdersUnitOfWork>();
        
        productCatalogMock
            .Setup(x => x.GetByIdAsync(
                productId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        Order? capturedOrder = null;

        orderRepositoryMock
            .Setup(x => x.AddAsync(
                It.IsAny<Order>(),
                It.IsAny<CancellationToken>()))
            .Callback<Order, CancellationToken>(
                (order, _) => capturedOrder = order)
            .Returns(Task.CompletedTask);

        var handler = new CreateOrderCommandHandler(
            productCatalogMock.Object,
            inventoryServiceMock.Object,
            orderRepositoryMock.Object,
            unitOfWorkMock.Object,
            new TestOrdersTransaction());

        //Act
        var orderId = await handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, orderId);

        Assert.NotNull(capturedOrder);
        Assert.Equal(orderId, capturedOrder.Id);
        Assert.Equal(customerId, capturedOrder.CustomerId);
        Assert.Single(capturedOrder.Items);
        Assert.Equal(199.98m, capturedOrder.TotalAmount);

        inventoryServiceMock.Verify(
            x => x.ReserveAsync(
                productId,
                2,
                It.IsAny<CancellationToken>()),
            Times.Once);

        orderRepositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Order>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}