using OrderManagement.Modules.Orders.Domain.Entities;
using OrderManagement.Modules.Orders.Domain.Enums;
using OrderManagement.Modules.Orders.Domain.Exceptions;

namespace OrderManagement.Modules.Orders.UnitTests;

public sealed class OrderTests
{
    [Fact]
    public void Constructor_WhenDataIsValid_ShouldCreateAwaitingPaymentOrder()
    {
        var customerId = Guid.NewGuid();

        var order = new Order(
            customerId,
            "PLN");

        Assert.NotEqual(Guid.Empty, order.Id);
        Assert.Equal(customerId, order.CustomerId);
        Assert.Equal(OrderStatus.AwaitingPayment, order.Status);
        Assert.Equal("PLN", order.Currency);
        Assert.Empty(order.Items);
        Assert.Equal(0m, order.TotalAmount);
    }

    [Fact]
    public void AddItem_WhenDataIsValid_ShouldAddItemAndCalculateTotalAmount()
    {
        var order = new Order(
            Guid.NewGuid(),
            "PLN");

        order.AddItem(
            Guid.NewGuid(),
            "SKU-001",
            "Test Product",
            100m,
            "PLN",
            3);

        Assert.Single(order.Items);
        Assert.Equal(300m, order.TotalAmount);
    }

    [Fact]
    public void AddItem_WhenProductAlreadyExists_ShouldThrowDuplicateOrderItemException()
    {
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
            1);

        Assert.Throws<DuplicateOrderItemException>(() =>
            order.AddItem(
                productId,
                "SKU-001",
                "Test Product",
                100m,
                "PLN",
                1));
    }

    [Fact]
    public void AddItem_WhenCurrencyDoesNotMatchOrder_ShouldThrowOrderCurrencyMismatchException()
    {
        var order = new Order(
            Guid.NewGuid(),
            "PLN");

        Assert.Throws<OrderCurrencyMismatchException>(() =>
            order.AddItem(
                Guid.NewGuid(),
                "SKU-001",
                "Test Product",
                100m,
                "EUR",
                1));
    }

    [Fact]
    public void AddItem_WhenOrderIsNotAwaitingPayment_ShouldThrowInvalidOrderStatusTransitionException()
    {
        var order = new Order(
            Guid.NewGuid(),
            "PLN");

        order.MarkAsPaid();

        Assert.Throws<InvalidOrderStatusTransitionException>(() =>
            order.AddItem(
                Guid.NewGuid(),
                "SKU-001",
                "Test Product",
                100m,
                "PLN",
                1));
    }

    [Fact]
    public void OrderLifecycle_WhenTransitionsAreValid_ShouldReachDeliveredStatus()
    {
        var order = new Order(
            Guid.NewGuid(),
            "PLN");

        order.MarkAsPaid();
        order.StartProcessing();
        order.MarkReadyForShipment();
        order.MarkAsShipped();
        order.MarkAsDelivered();

        Assert.Equal(OrderStatus.Delivered, order.Status);
    }
}
