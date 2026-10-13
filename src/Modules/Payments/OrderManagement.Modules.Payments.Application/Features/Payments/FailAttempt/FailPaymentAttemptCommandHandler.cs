using MediatR;
using OrderManagement.Modules.Payments.Application.Abstractions;
using OrderManagement.Modules.Payments.Application.Exceptions;

namespace OrderManagement.Modules.Payments.Application.Features.Payments.FailAttempt;

internal sealed class FailPaymentAttemptCommandHandler : IRequestHandler<FailPaymentAttemptCommand>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentsUnitOfWork _unitOfWork;

    public FailPaymentAttemptCommandHandler(
        IPaymentRepository paymentRepository,
        IPaymentsUnitOfWork unitOfWork)
    {
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        FailPaymentAttemptCommand request,
        CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(request.PaymentId, cancellationToken);

        if (payment is null)
            throw new PaymentNotFoundException(request.PaymentId);

        var attempt = payment.Attempts.FirstOrDefault(a => a.Id == request.AttemptId);

        if (attempt is null)
            throw new PaymentAttemptNotFoundException(request.AttemptId);

        attempt.Fail(request.FailureReason);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
