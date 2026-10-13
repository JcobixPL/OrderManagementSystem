using MediatR;

namespace OrderManagement.Modules.Payments.Application.Features.Payments.FailAttempt;

public sealed record FailPaymentAttemptCommand(
    Guid PaymentId,
    Guid AttemptId,
    string FailureReason) : IRequest;
