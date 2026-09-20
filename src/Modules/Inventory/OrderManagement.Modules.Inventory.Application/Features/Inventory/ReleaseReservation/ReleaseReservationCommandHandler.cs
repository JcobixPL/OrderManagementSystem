using MediatR;
using OrderManagement.Modules.Inventory.Application.Abstractions;
using OrderManagement.Modules.Inventory.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.ReleaseReservation;

internal sealed class ReleaseReservationCommandHandler
    : IRequestHandler<ReleaseReservationCommand>
{
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IInventoryUnitOfWork _unitOfWork;

    public ReleaseReservationCommandHandler(IInventoryRepository inventoryRepository, IInventoryUnitOfWork inventoryUnitOfWork)
    {
        _inventoryRepository = inventoryRepository;
        _unitOfWork = inventoryUnitOfWork;
    }

    public async Task Handle(
        ReleaseReservationCommand request,
        CancellationToken cancellationToken)
    {
        var inventoryItem = await _inventoryRepository.GetByProductIdAsync(request.ProductId, cancellationToken);

        if (inventoryItem is null)
            throw new InventoryItemNotFoundException(request.ProductId);

        inventoryItem.ReleaseReservation(request.Quantity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
