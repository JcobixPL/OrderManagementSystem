using MediatR;
using OrderManagement.Modules.Orders.Application.Abstractions;
using OrderManagement.Modules.Orders.Application.Exceptions;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.MarkAsPaid;

internal sealed class MarkOrderAsPaidCommandHandler
    : IRequestHandler<MarkOrderAsPaidCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrdersUnitOfWork _unitOfWork;

    public MarkOrderAsPaidCommandHandler(IOrderRepository orderRepository, IOrdersUnitOfWork ordersUnitOfWork)
    {
        _orderRepository = orderRepository;
        _unitOfWork = ordersUnitOfWork;
    }

    public async Task Handle(
        MarkOrderAsPaidCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(
            request.OrderId,
            cancellationToken);

        if (order is null)
            throw new OrderNotFoundException(request.OrderId);

        order.MarkAsPaid();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
