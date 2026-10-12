using MediatR;

namespace OrderManagement.Modules.Payments.Application.Features.Payments.Create;

public sealed record CreatePaymentCommand(Guid OrderId) : IRequest<Guid>;
