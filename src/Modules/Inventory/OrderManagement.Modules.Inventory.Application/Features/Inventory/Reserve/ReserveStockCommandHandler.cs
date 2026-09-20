using MediatR;
using OrderManagement.Modules.Inventory.Application.Abstractions;
using OrderManagement.Modules.Inventory.Application.Exceptions;

namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.Reserve;

internal sealed class ReserveStockCommandHandler
    : IRequestHandler<ReserveStockCommand>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IInventoryUnitOfWork _unitOfWork;

    public ReserveStockCommandHandler(IInventoryRepository inventoryRepository, IInventoryUnitOfWork inventoryUnitOfWork)
    {
        _inventoryRepository = inventoryRepository;
        _unitOfWork = inventoryUnitOfWork;
    }

    public async Task Handle(
        ReserveStockCommand request,
        CancellationToken cancellationToken)
    {
        var inventoryItem = await _inventoryRepository.GetByProductIdAsync(request.ProductId, cancellationToken);

        if (inventoryItem is null)
            throw new InventoryItemNotFoundException(request.ProductId);

        inventoryItem.Reserve(request.Quantity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
