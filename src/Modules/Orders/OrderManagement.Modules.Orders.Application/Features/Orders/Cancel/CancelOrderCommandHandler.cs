using MediatR;
using OrderManagement.Modules.Orders.Application.Abstractions;
using OrderManagement.Modules.Orders.Application.Exceptions;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.Cancel;

internal sealed class CancelOrderCommandHandler
    : IRequestHandler<CancelOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrdersUnitOfWork _unitOfWork;
    private readonly IInventoryService _inventoryService;
    private readonly IOrdersTransaction _transaction;

    public CancelOrderCommandHandler(
        IOrderRepository orderRepository,
        IOrdersUnitOfWork unitOfWork,
        IInventoryService inventoryService,
        IOrdersTransaction transaction)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _inventoryService = inventoryService;
        _transaction = transaction;
    }

    public async Task Handle(
        CancelOrderCommand request,
        CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(
            request.OrderId,
            cancellationToken);

        if (order is null)
            throw new OrderNotFoundException(request.OrderId);
        
        await _transaction.ExecuteAsync(
            async ct =>
            {
                order.Cancel();

                foreach (var item in order.Items)
                {
                    await _inventoryService.ReleaseReservationAsync(
                        item.ProductId,
                        item.Quantity,
                        ct);
                }

                await _unitOfWork.SaveChangesAsync(ct);

                return true;
            },
            cancellationToken);
    }
}