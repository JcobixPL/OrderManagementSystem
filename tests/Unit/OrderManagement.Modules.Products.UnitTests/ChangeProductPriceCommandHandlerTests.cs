using Moq;
using OrderManagement.Modules.Products.Application.Abstractions;
using OrderManagement.Modules.Products.Application.Exceptions;
using OrderManagement.Modules.Products.Application.Features.Products.ChangePrice;
using OrderManagement.Modules.Products.Domain.Entities;

namespace OrderManagement.Modules.Products.UnitTests;

public sealed class ChangeProductPriceCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenProductExists_ShouldChangePriceAndSaveChanges()
    {
        var productId = Guid.NewGuid();

        var product = new Product(
            "SKU-001",
            "Test Product",
            "Test description",
            Money.Create(100m, "PLN"));

        var repositoryMock = new Mock<IProductRepository>();
        var unitOfWorkMock = new Mock<IProductsUnitOfWork>();

        repositoryMock
            .Setup(x => x.GetByIdAsync(
                productId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var handler = new ChangeProductPriceCommandHandler(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new ChangeProductPriceCommand(
            productId,
            150m,
            "PLN");

        await handler.Handle(
            command,
            CancellationToken.None);

        Assert.Equal(150m, product.Price.Amount);
        Assert.Equal("PLN", product.Price.Currency);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenProductDoesNotExist_ShouldThrowProductNotFoundException()
    {
        var productId = Guid.NewGuid();

        var repositoryMock = new Mock<IProductRepository>();
        var unitOfWorkMock = new Mock<IProductsUnitOfWork>();

        repositoryMock
            .Setup(x => x.GetByIdAsync(
                productId,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var handler = new ChangeProductPriceCommandHandler(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new ChangeProductPriceCommand(
            productId,
            150m,
            "PLN");

        await Assert.ThrowsAsync<ProductNotFoundException>(
            () => handler.Handle(
                command,
                CancellationToken.None));

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
