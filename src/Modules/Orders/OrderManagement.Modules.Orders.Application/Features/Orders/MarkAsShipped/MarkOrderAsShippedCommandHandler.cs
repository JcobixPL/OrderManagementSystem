using MediatR;
using OrderManagement.Modules.Orders.Application.Abstractions;
using OrderManagement.Modules.Orders.Application.Exceptions;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.MarkAsShipped;

internal sealed class MarkOrderAsShippedCommandHandler
    : IRequestHandler<MarkOrderAsShippedCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrdersUnitOfWork _unitOfWork;
    private readonly IInventoryService _inventoryService;
    private readonly IOrdersTransaction _transaction;

    public MarkOrderAsShippedCommandHandler(
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
        MarkOrderAsShippedCommand request,
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
                order.MarkAsShipped();

                foreach (var item in order.Items)
                {
                    await _inventoryService.ConfirmRemovalAsync(
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
