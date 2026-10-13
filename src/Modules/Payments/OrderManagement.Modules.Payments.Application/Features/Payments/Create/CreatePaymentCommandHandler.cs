using MediatR;
using OrderManagement.Modules.Payments.Application.Abstractions;
using OrderManagement.Modules.Payments.Application.Exceptions;
using OrderManagement.Modules.Payments.Application.Features.Payments.Create;
using OrderManagement.Modules.Payments.Domain.Entities;

namespace OrderManagement.Modules.Payments.Application.Features.Payments.Create;

internal sealed class CreatePaymentCommandHandler
    : IRequestHandler<CreatePaymentCommand, Guid>
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentsUnitOfWork _unitOfWork;
    private readonly IOrderPaymentDetailsProvider _orderPaymentDetailsProvider;
    private readonly IPaymentCurrentUser _currentUser;

    public CreatePaymentCommandHandler(
        IPaymentRepository paymentRepository,
        IPaymentsUnitOfWork unitOfWork,
        IOrderPaymentDetailsProvider orderPaymentDetailsProvider,
        IPaymentCurrentUser paymentCurrentUser)
    {
        _paymentRepository = paymentRepository;
        _unitOfWork = unitOfWork;
        _orderPaymentDetailsProvider = orderPaymentDetailsProvider;
        _currentUser = paymentCurrentUser;
    }

    public async Task<Guid> Handle(
        CreatePaymentCommand request,
        CancellationToken cancellationToken)
    {
        var existingPayment = await _paymentRepository.GetByOrderIdAsync(
            request.OrderId,
            cancellationToken);

        if (existingPayment is not null)
        {
            throw new PaymentAlreadyExistsException(request.OrderId);
        }

        var order = await _orderPaymentDetailsProvider.GetAsync(
            request.OrderId,
            cancellationToken);

        if (order is null)
        {
            throw new InvalidOperationException(
                $"Order '{request.OrderId}' was not found.");
        }

        if (order.CustomerId != _currentUser.UserId)
            throw new PaymentAccessDeniedException();
        
        if (order.Status != "AwaitingPayment")
        {
            throw new OrderNotPayableException(
                order.OrderId,
                order.Status);
        }

        var payment = new Payment(
            order.OrderId,
            order.CustomerId,
            order.TotalAmount,
            order.Currency);

        await _paymentRepository.AddAsync(
            payment,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return payment.Id;
    }
}