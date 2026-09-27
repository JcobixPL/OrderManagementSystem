using MediatR;
using OrderManagement.Modules.Orders.Application.Abstractions;
using OrderManagement.Modules.Orders.Application.Exceptions;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.MarkAsDelivered;

internal sealed class MarkOrderAsDeliveredCommandHandler
    : IRequestHandler<MarkOrderAsDeliveredCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrdersUnitOfWork _unitOfWork;

    public MarkOrderAsDeliveredCommandHandler(
        IOrderRepository orderRepository,
        IOrdersUnitOfWork unitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(
        MarkOrderAsDeliveredCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(
            request.OrderId,
            cancellationToken);

        if (order is null)
            throw new OrderNotFoundException(request.OrderId);
        
        order.MarkAsDelivered();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}