using MediatR;
using OrderManagement.Modules.Orders.Application.Abstractions;
using OrderManagement.Modules.Orders.Application.Exceptions;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.GetPaymentDetails;

internal sealed class GetOrderPaymentDetailsQueryHandler
    : IRequestHandler<GetOrderPaymentDetailsQuery, OrderPaymentDetailsDto>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderPaymentDetailsQueryHandler(
        IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderPaymentDetailsDto> Handle(
        GetOrderPaymentDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(
            request.OrderId,
            cancellationToken);

        if (order is null)
        {
            throw new OrderNotFoundException(request.OrderId);
        }

        return new OrderPaymentDetailsDto(
            order.Id,
            order.CustomerId,
            order.TotalAmount,
            order.Currency,
            order.Status.ToString());
    }
}