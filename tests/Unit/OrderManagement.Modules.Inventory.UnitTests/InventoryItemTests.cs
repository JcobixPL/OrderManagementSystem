using OrderManagement.Modules.Inventory.Domain.Entities;
using OrderManagement.Modules.Inventory.Domain.Exceptions;

namespace OrderManagement.Modules.Inventory.UnitTests;

public sealed class InventoryItemTests
{
    [Fact]
    public void Reserve_WhenStockIsAvailable_ShouldMoveQuantityToReserved()
    {
        var inventoryItem = new InventoryItem(
            Guid.NewGuid(),
            10);

        inventoryItem.Reserve(3);

        Assert.Equal(7, inventoryItem.AvailableQuantity);
        Assert.Equal(3, inventoryItem.ReservedQuantity);
    }

    [Fact]
    public void AddStock_WhenQuantityIsValid_ShouldIncreaseAvailableQuantity()
    {
        var inventoryItem = new InventoryItem(
            Guid.NewGuid(),
            10);

        inventoryItem.AddStock(5);

        Assert.Equal(15, inventoryItem.AvailableQuantity);
    }

    [Fact]
    public void Reserve_WhenQuantityExceedsAvailableStock_ShouldThrow()
    {
        var inventoryItem = new InventoryItem(
            Guid.NewGuid(),
            5);

        Assert.Throws<InsufficientStockException>(
            () => inventoryItem.Reserve(6));
    }

    [Fact]
    public void ReleaseReservation_WhenQuantityIsValid_ShouldReturnStockToAvailable()
    {
        var inventoryItem = new InventoryItem(
            Guid.NewGuid(),
            10);

        inventoryItem.Reserve(4);

        inventoryItem.ReleaseReservation(2);

        Assert.Equal(8, inventoryItem.AvailableQuantity);
        Assert.Equal(2, inventoryItem.ReservedQuantity);
    }

    [Fact]
    public void ConfirmRemoval_WhenQuantityIsValid_ShouldDecreaseReservedQuantity()
    {
        var inventoryItem = new InventoryItem(
           Guid.NewGuid(),
           10);

        inventoryItem.Reserve(4);

        inventoryItem.ConfirmRemoval(3);

        Assert.Equal(6, inventoryItem.AvailableQuantity);
        Assert.Equal(1, inventoryItem.ReservedQuantity);
    }
}
