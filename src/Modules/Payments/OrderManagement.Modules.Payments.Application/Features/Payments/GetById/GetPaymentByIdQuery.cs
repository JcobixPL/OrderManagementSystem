using MediatR;
using OrderManagement.Modules.Payments.Application.DTOs;

namespace OrderManagement.Modules.Payments.Application.Features.Payments.GetById;

public sealed record GetPaymentByIdQuery(Guid PaymentId)
    : IRequest<PaymentDto>;