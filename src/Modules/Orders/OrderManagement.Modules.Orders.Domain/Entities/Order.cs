using OrderManagement.Modules.Orders.Domain.Enums;

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
            throw new InvalidOperationException(
                "Items can only be added to an order awaiting payment.");
        }

        if (!string.Equals(
            Currency,
            currency,
            StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Order item currency must match order currency.");
        }

        if (_items.Any(x => x.ProductId == productId))
        {
            throw new InvalidOperationException(
                "Product already exists in the order.");
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
            throw new InvalidOperationException(
                "Only an order awaiting payment can be marked as paid.");
        }

        Status = OrderStatus.Paid;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void StartProcessing()
    {
        if (Status != OrderStatus.Paid)
        {
            throw new InvalidOperationException(
                "Only a paid order can start processing.");
        }

        Status = OrderStatus.Processing;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkReadyForShipment()
    {
        if (Status != OrderStatus.Processing)
        {
            throw new InvalidOperationException(
                "Only an order being processed can be marked as ready for shipment.");
        }

        Status = OrderStatus.ReadyForShipment;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkAsShipped()
    {
        if (Status != OrderStatus.ReadyForShipment)
        {
            throw new InvalidOperationException(
                "Only an order ready for shipment can be marked as shipped.");
        }

        Status = OrderStatus.Shipped;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkAsDelivered()
    {
        if (Status != OrderStatus.Shipped)
        {
            throw new InvalidOperationException(
                "Only a shipped order can be marked as delivered.");
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
            throw new InvalidOperationException(
                "The order cannot be cancelled in its current status.");
        }

        Status = OrderStatus.Cancelled;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
