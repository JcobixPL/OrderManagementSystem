using OrderManagement.Modules.Orders.Application.Features.Orders.Create;

namespace OrderManagement.Modules.Orders.UnitTests;

public sealed class CreateOrderCommandValidatorTests
{
    [Fact]
    public async Task ValidateAsync_ShouldHaveError_WhenOrderContainsDuplicateProducts()
    {
        // Arrange
        var productId = Guid.NewGuid();

        var command = new CreateOrderCommand(
            Guid.NewGuid(),
            new[]
            {
                new CreateOrderItem(productId, 1),
                new CreateOrderItem(productId, 2)
            });

        var validator = new CreateOrderCommandValidator();

        // Act
        var result = await validator.ValidateAsync(command);

        // Assert
        Assert.False(result.IsValid);

        Assert.Contains(
            result.Errors,
            error => error.ErrorMessage ==
                        "Order cannot contain duplicate products.");
    }
}
