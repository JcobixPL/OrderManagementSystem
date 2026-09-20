using MediatR;
using OrderManagement.Modules.Inventory.Application.Abstractions;
using OrderManagement.Modules.Inventory.Application.Exceptions;
using OrderManagement.Modules.Inventory.Domain.Entities;

namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.Create;

internal sealed class CreateInventoryItemCommandHandler : IRequestHandler<CreateInventoryItemCommand, Guid>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IInventoryUnitOfWork _unitOfWork;

    public CreateInventoryItemCommandHandler(IInventoryRepository inventoryRepository, IInventoryUnitOfWork unitOfWork)
    {
        _inventoryRepository = inventoryRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        CreateInventoryItemCommand request,
        CancellationToken cancellationToken)
    {
        var alreadyExists = await _inventoryRepository.ExistsByProductIdAsync(request.ProductId, cancellationToken);

        if (alreadyExists)
            throw new InventoryItemAlreadyExistsException(request.ProductId);

        var inventoryItem = new InventoryItem(
            request.ProductId,
            request.InitialQuantity);

        await _inventoryRepository.AddAsync(inventoryItem, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return inventoryItem.Id;
    }
}
