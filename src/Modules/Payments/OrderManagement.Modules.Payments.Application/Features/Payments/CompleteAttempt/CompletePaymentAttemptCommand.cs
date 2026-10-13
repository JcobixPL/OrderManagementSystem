using MediatR;

namespace OrderManagement.Modules.Payments.Application.Features.Payments.CompleteAttempt;

public sealed record CompletePaymentAttemptCommand(
    Guid PaymentId,
    Guid AttemptId,
    string ExternalPaymentId) : IRequest;