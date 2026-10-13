using MediatR;
using OrderManagement.Modules.Payments.Application.Abstractions;
using OrderManagement.Modules.Payments.Application.DTOs;
using OrderManagement.Modules.Payments.Application.Exceptions;

namespace OrderManagement.Modules.Payments.Application.Features.Payments.GetById;

internal sealed class GetPaymentByIdQueryHandler
    : IRequestHandler<GetPaymentByIdQuery, PaymentDto>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentCurrentUser _currentUser;

    public GetPaymentByIdQueryHandler(
        IPaymentRepository paymentRepository,
        IPaymentCurrentUser currentUser)
    {
        _paymentRepository = paymentRepository;
        _currentUser = currentUser;
    }

    public async Task<PaymentDto> Handle(
        GetPaymentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(
            request.PaymentId,
            cancellationToken);

        if (payment is null)
            throw new PaymentNotFoundException(request.PaymentId);

        if (payment.CustomerId != _currentUser.UserId)
            throw new PaymentAccessDeniedException();

        return new PaymentDto(
            payment.Id,
            payment.OrderId,
            payment.Amount,
            payment.Currency,
            payment.Status.ToString(),
            payment.CreatedAt,
            payment.PaidAt,
            payment.Attempts
                .Select(attempt => new PaymentAttemptDto(
                    attempt.Id,
                    attempt.Status.ToString(),
                    attempt.ExternalPaymentId,
                    attempt.FailureReason,
                    attempt.CreatedAt,
                    attempt.CompletedAt))
                .ToList());
    }
}