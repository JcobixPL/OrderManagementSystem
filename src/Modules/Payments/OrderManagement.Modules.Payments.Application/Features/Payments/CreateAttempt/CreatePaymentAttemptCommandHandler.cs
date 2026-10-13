using MediatR;
using OrderManagement.Modules.Payments.Application.Abstractions;
using OrderManagement.Modules.Payments.Application.Exceptions;

namespace OrderManagement.Modules.Payments.Application.Features.Payments.CreateAttempt;

internal sealed class CreatePaymentAttemptCommandHandler
    : IRequestHandler<CreatePaymentAttemptCommand, Guid>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentsUnitOfWork _unitOfWork;
    private readonly IPaymentCurrentUser _currentUser;

    public CreatePaymentAttemptCommandHandler(
        IPaymentRepository paymentRepository,
        IPaymentsUnitOfWork unitOfWork,
        IPaymentCurrentUser currentUser)
    {
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(
        CreatePaymentAttemptCommand request,
        CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetByIdAsync(request.PaymentId, cancellationToken);

        if (payment is null)
            throw new PaymentNotFoundException(request.PaymentId);

        if (payment.CustomerId != _currentUser.UserId)
            throw new PaymentAccessDeniedException();

        var attempt = payment.CreateAttempt();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return attempt.Id;
    }
}
