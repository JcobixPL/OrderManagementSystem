using MediatR;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.Create;

public sealed record CreateOrderCommand(
    Guid CustomerId,
    IReadOnlyCollection<CreateOrderItem> Items)
        : IRequest<Guid>;
