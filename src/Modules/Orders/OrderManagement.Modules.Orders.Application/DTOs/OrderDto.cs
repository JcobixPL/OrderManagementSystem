namespace OrderManagement.Modules.Orders.Application.DTOs;

public sealed record OrderDto(
    Guid Id,
    Guid CustomerId,
    string Status,
    decimal TotalAmount,
    string Currency,
    DateTimeOffset CreatedAt,
    IReadOnlyCollection<OrderItemDto> Items);

public sealed record OrderItemDto(
    Guid ProductId,
    string ProductSku,
    string ProductName,
    decimal UnitPrice,
    string Currency,
    int Quantity,
    decimal TotalPrice);