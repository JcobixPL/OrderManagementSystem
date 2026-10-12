using MediatR;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.GetPaymentDetails;

public sealed record GetOrderPaymentDetailsQuery(Guid OrderId)
    : IRequest<OrderPaymentDetailsDto>;