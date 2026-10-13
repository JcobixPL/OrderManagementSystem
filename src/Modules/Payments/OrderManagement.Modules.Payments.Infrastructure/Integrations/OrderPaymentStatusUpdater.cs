using MediatR;
using OrderManagement.Modules.Orders.Application.Features.Orders.MarkAsPaid;
using OrderManagement.Modules.Payments.Application.Abstractions;

namespace OrderManagement.Modules.Payments.Infrastructure.Integrations;

internal sealed class OrderPaymentStatusUpdater
    : IOrderPaymentStatusUpdater
{
    private readonly ISender _sender;

    public OrderPaymentStatusUpdater(ISender sender)
    {
        _sender = sender;
    }

    public async Task MarkAsPaidAsync(
        Guid orderId,
        CancellationToken cancellationToken = default)
    {
        await _sender.Send(
            new MarkOrderAsPaidCommand(orderId),
            cancellationToken);
    }
}