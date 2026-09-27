using MediatR;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.Cancel;

public sealed record CancelOrderCommand(Guid OrderId) : IRequest;