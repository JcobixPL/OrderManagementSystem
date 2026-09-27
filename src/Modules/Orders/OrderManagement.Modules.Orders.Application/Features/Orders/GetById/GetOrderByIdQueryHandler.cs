using MediatR;
using OrderManagement.Modules.Orders.Application.Abstractions;
using OrderManagement.Modules.Orders.Application.DTOs;
using OrderManagement.Modules.Orders.Application.Exceptions;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.GetById;

internal sealed class GetOrderByIdQueryHandler
    : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderDto> Handle(
        GetOrderByIdQuery request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
            throw new OrderNotFoundException(request.OrderId);

        return new OrderDto(
            order.Id,
            order.CustomerId,
            order.Status.ToString(),
            order.TotalAmount,
            order.Currency,
            order.CreatedAt,
            order.Items
                .Select(item => new OrderItemDto(
                    item.ProductId,
                    item.ProductSku,
                    item.ProductName,
                    item.UnitPrice,
                    item.Currency,
                    item.Quantity,
                    item.TotalPrice))
                .ToList());
    }
}
