using OrderManagement.Modules.Products.Domain.Entities;

namespace OrderManagement.Modules.Products.UnitTests;

public sealed class ProductTests
{
    [Fact]
    public void ChangePrice_WhenPriceIsValid_ShouldUpdatePrice()
    {
        var product = new Product(
            "SKU-001",
            "Test product",
            "Test decription",
            Money.Create(100m, "PLN"));

        product.ChangePrice(Money.Create(150m, "PLN"));

        Assert.Equal(150m, product.Price.Amount);
        Assert.Equal("PLN", product.Price.Currency);
    }

    [Fact]
    public void Rename_WhenNameIsValid_ShouldUpdateName()
    {
        var product = new Product(
            "SKU-001",
            "Old Name",
            "Test description",
            Money.Create(100m, "PLN"));

        product.Rename("New Name");

        Assert.Equal("New Name", product.Name);
    }

    [Fact]
    public void Deactivate_WhenProductIsActive_ShouldSetIsActiveToFalse()
    {
        var product = new Product(
            "SKU-001",
            "Test Product",
            "Test description",
            Money.Create(100m, "PLN"));

        product.Deactivate();

        Assert.False(product.IsActive);
    }

    [Fact]
    public void Activate_WhenProductIsInactive_ShouldSetIsActiveToTrue()
    {
        var product = new Product(
            "SKU-001",
            "Test Product",
            "Test description",
            Money.Create(100m, "PLN"));

        product.Deactivate();

        product.Activate();

        Assert.True(product.IsActive);
    }
}
