namespace OrderManagement.Modules.Orders.Application.Features.Orders.Create;

public sealed record CreateOrderItem(
    Guid ProductId,
    int Quantity);
