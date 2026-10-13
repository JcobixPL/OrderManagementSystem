using MediatR;
using OrderManagement.Modules.Orders.Application.Features.Orders.GetPaymentDetails;
using OrderManagement.Modules.Payments.Application.Abstractions;

namespace OrderManagement.Modules.Payments.Infrastructure.Integrations;

internal sealed class OrderPaymentDetailsProvider
    : IOrderPaymentDetailsProvider
{
    private readonly ISender _sender;

    public OrderPaymentDetailsProvider(ISender sender)
    {
        _sender = sender;
    }

    public async Task<OrderPaymentDetails?> GetAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        var order = await _sender.Send(
            new GetOrderPaymentDetailsQuery(orderId),
            cancellationToken);

        return new OrderPaymentDetails(
            order.OrderId,
            order.CustomerId,
            order.TotalAmount,
            order.Currency,
            order.Status);
    }
}