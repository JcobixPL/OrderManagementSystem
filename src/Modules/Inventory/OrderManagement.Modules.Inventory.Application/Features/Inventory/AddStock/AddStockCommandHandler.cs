using MediatR;
using OrderManagement.Modules.Inventory.Application.Abstractions;
using OrderManagement.Modules.Inventory.Application.Exceptions;

namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.AddStock;

internal sealed class AddStockCommandHandler : IRequestHandler<AddStockCommand>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IInventoryUnitOfWork _unitOfWork;

    public AddStockCommandHandler(IInventoryRepository inventoryRepository, IInventoryUnitOfWork inventoryUnitOfWork)
    {
        _inventoryRepository = inventoryRepository;
        _unitOfWork = inventoryUnitOfWork;
    }

    public async Task Handle(
        AddStockCommand request,
        CancellationToken cancellationToken)
    {
        var inventoryItem = await _inventoryRepository.GetByProductIdAsync(request.ProductId, cancellationToken);

        if (inventoryItem is null)
            throw new InventoryItemNotFoundException(request.ProductId);

        inventoryItem.AddStock(request.Quantity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
