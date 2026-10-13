using MediatR;

namespace OrderManagement.Modules.Payments.Application.Features.Payments.CreateAttempt;

public sealed record CreatePaymentAttemptCommand(Guid PaymentId) : IRequest<Guid>;