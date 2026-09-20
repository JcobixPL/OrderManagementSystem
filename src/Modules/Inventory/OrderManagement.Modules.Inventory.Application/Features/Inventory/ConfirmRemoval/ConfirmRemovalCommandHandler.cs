using MediatR;
using OrderManagement.Modules.Inventory.Application.Abstractions;
using OrderManagement.Modules.Inventory.Application.Exceptions;
using OrderManagement.Modules.Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.ConfirmRemoval;

internal sealed class ConfirmRemovalCommandHandler
    : IRequestHandler<ConfirmRemovalCommand>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IInventoryUnitOfWork _unitOfWork;

    public ConfirmRemovalCommandHandler(IInventoryRepository inventoryRepository, IInventoryUnitOfWork inventoryUnitOfWork)
    {
        _inventoryRepository = inventoryRepository;
        _unitOfWork = inventoryUnitOfWork;
    }

    public async Task Handle(
        ConfirmRemovalCommand request,
        CancellationToken cancellationToken)
    {
        var inventoryItem = await _inventoryRepository.GetByProductIdAsync(request.ProductId, cancellationToken);

        if (inventoryItem is null)
            throw new InventoryItemNotFoundException(request.ProductId);

        inventoryItem.ConfirmRemoval(request.Quantity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
