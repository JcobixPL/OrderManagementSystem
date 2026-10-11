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
    private readonly ICurrentUser _currentUser;

    public CancelOrderCommandHandler(
        IOrderRepository orderRepository,
        IOrdersUnitOfWork unitOfWork,
        IInventoryService inventoryService,
        IOrdersTransaction transaction,
        ICurrentUser currentUser)
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
        _inventoryService = inventoryService;
        _transaction = transaction;
        _currentUser = currentUser;
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

        if (order.CustomerId != _currentUser.UserId)
        {
            throw new OrderAccessDeniedException();
        }

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