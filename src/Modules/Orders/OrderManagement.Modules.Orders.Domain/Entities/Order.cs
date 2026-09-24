using OrderManagement.Modules.Orders.Domain.Enums;
using OrderManagement.Modules.Orders.Domain.Exceptions;

namespace OrderManagement.Modules.Orders.Domain.Entities;

public sealed class Order
{
    private readonly List<OrderItem> _items = [];

    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public decimal TotalAmount => _items.Sum(x => x.TotalPrice);
    public string Currency { get; private set; } = null!;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    private Order()
    {
    }

    public Order(
        Guid customerId,
        string currency)
    {
        if (customerId == Guid.Empty)
        {
            throw new ArgumentException(
                "Customer ID cannot be empty.",
                nameof(customerId));
        }

        if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
        {
            throw new ArgumentException(
                "Currency must contain exactly 3 characters.",
                nameof(currency));
        }

        Id = Guid.NewGuid();
        CustomerId = customerId;
        Currency = currency.Trim().ToUpperInvariant();
        Status = OrderStatus.AwaitingPayment;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void AddItem(
    Guid productId,
    string productSku,
    string productName,
    decimal unitPrice,
    string currency,
    int quantity)
    {
        if (Status != OrderStatus.AwaitingPayment)
        {
            throw new InvalidOrderStatusTransitionException(
                Status, "AddItem");
        }

        if (!string.Equals(
            Currency,
            currency,
            StringComparison.OrdinalIgnoreCase))
        {
            throw new OrderCurrencyMismatchException(
                Currency,
                currency);
        }

        if (_items.Any(x => x.ProductId == productId))
        {
            throw new DuplicateOrderItemException(productId);
        }

        var orderItem = new OrderItem(
            productId,
            productSku,
            productName,
            unitPrice,
            currency,
            quantity);

        _items.Add(orderItem);
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkAsPaid()
    {
        if (Status != OrderStatus.AwaitingPayment)
        {
            throw new InvalidOrderStatusTransitionException(
                Status,
                "MarkAsPaid");
        }

        Status = OrderStatus.Paid;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void StartProcessing()
    {
        if (Status != OrderStatus.Paid)
        {
            throw new InvalidOrderStatusTransitionException(
                Status,
                "StartProcessing");
        }

        Status = OrderStatus.Processing;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkReadyForShipment()
    {
        if (Status != OrderStatus.Processing)
        {
            throw new InvalidOrderStatusTransitionException(
                Status,
                "MarkReadyForShipment");
        }

        Status = OrderStatus.ReadyForShipment;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkAsShipped()
    {
        if (Status != OrderStatus.ReadyForShipment)
        {
            throw new InvalidOrderStatusTransitionException(
                Status,
                "MarkAsShipped");
        }

        Status = OrderStatus.Shipped;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkAsDelivered()
    {
        if (Status != OrderStatus.Shipped)
        {
            throw new InvalidOrderStatusTransitionException(
                Status,
                "MarkAsDelivered");
        }

        Status = OrderStatus.Delivered;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Cancel()
    {
        if (Status is OrderStatus.Shipped
            or OrderStatus.Delivered
            or OrderStatus.Cancelled
            or OrderStatus.Expired)
        {
            throw new InvalidOrderStatusTransitionException(
                Status,
                "Cancel");
        }

        Status = OrderStatus.Cancelled;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
