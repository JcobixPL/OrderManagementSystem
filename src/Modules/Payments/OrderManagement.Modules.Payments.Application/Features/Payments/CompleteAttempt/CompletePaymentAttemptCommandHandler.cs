using MediatR;
using OrderManagement.Modules.Payments.Application.Abstractions;
using OrderManagement.Modules.Payments.Application.Exceptions;

namespace OrderManagement.Modules.Payments.Application.Features.Payments.CompleteAttempt;

internal sealed class CompletePaymentAttemptCommandHandler
    : IRequestHandler<CompletePaymentAttemptCommand>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentsUnitOfWork _unitOfWork;
    private readonly IOrderPaymentStatusUpdater _orderPaymentStatusUpdater;
    private readonly IPaymentCurrentUser _currentUser;

    public CompletePaymentAttemptCommandHandler(
        IPaymentRepository paymentRepository,
        IPaymentsUnitOfWork unitOfWork,
        IOrderPaymentStatusUpdater orderPaymentStatusUpdater,
        IPaymentCurrentUser paymentCurrentUser)
    {
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
        _orderPaymentStatusUpdater = orderPaymentStatusUpdater;
        _currentUser = paymentCurrentUser;
    }

    public async Task Handle(
        CompletePaymentAttemptCommand request,
        CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(
            request.PaymentId,
            cancellationToken);

        if (payment is null)
            throw new PaymentNotFoundException(request.PaymentId);

        if (payment.CustomerId != _currentUser.UserId)
            throw new PaymentAccessDeniedException();

        var attempt = payment.Attempts
            .FirstOrDefault(a => a.Id == request.AttemptId);

        if (attempt is null)
            throw new PaymentAttemptNotFoundException(request.AttemptId);

        attempt.Complete(request.ExternalPaymentId);

        payment.Complete(attempt);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _orderPaymentStatusUpdater.MarkAsPaidAsync(
            payment.OrderId,
            cancellationToken);
    }
}