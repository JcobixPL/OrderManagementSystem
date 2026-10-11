using MediatR;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.Create;

public sealed record CreateOrderCommand(
    IReadOnlyCollection<CreateOrderItem> Items)
        : IRequest<Guid>;
