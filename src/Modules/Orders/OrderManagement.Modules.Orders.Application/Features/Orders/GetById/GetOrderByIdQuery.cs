using MediatR;
using OrderManagement.Modules.Orders.Application.DTOs;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.GetById;

public sealed record GetOrderByIdQuery(Guid OrderId)
    : IRequest<OrderDto>;
