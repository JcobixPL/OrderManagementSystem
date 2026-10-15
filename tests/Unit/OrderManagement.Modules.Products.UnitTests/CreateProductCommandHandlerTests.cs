using Moq;
using OrderManagement.Modules.Products.Application.Abstractions;
using OrderManagement.Modules.Products.Application.Exceptions;
using OrderManagement.Modules.Products.Application.Features.Products.Create;
using OrderManagement.Modules.Products.Domain.Entities;

namespace OrderManagement.Modules.Products.UnitTests;

public sealed class CreateProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_WhenSkuIsUnique_ShouldCreateProductAndSaveChanges()
    {
        var repositoryMock = new Mock<IProductRepository>();
        var unitOfWorkMock = new Mock<IProductsUnitOfWork>();

        repositoryMock
            .Setup(x => x.ExistsBySkuAsync(
                "SKU-001",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new CreateProductCommandHandler(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new CreateProductCommand(
            "SKU-001",
            "Test Product",
            "Test description",
            100m,
            "PLN");

        var productId = await handler.Handle(
            command,
            CancellationToken.None);

        Assert.NotEqual(Guid.Empty, productId);

        repositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Domain.Entities.Product>(),
                It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenSkuAlreadyExists_ShouldThrowDuplicateProductSkuException()
    {
        var repositoryMock = new Mock<IProductRepository>();
        var unitOfWorkMock = new Mock<IProductsUnitOfWork>();

        repositoryMock
            .Setup(x => x.ExistsBySkuAsync(
                "SKU-001",
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new CreateProductCommandHandler(
            repositoryMock.Object,
            unitOfWorkMock.Object);

        var command = new CreateProductCommand(
            "SKU-001",
            "Test Product",
            "Test description",
            100m,
            "PLN");

        await Assert.ThrowsAsync<DuplicateProductSkuException>(
            () => handler.Handle(
                command,
                CancellationToken.None));

        repositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Product>(),
                It.IsAny<CancellationToken>()),
            Times.Never);

        unitOfWorkMock.Verify(
            x => x.SaveChangesAsync(
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
