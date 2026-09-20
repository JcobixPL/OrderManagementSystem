using MediatR;
using OrderManagement.Modules.Inventory.Application.Abstractions;
using OrderManagement.Modules.Inventory.Application.DTOs;
using OrderManagement.Modules.Inventory.Application.Exceptions;

namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.GetByProductId;

internal sealed class GetInventoryByProductIdQueryHandler
    : IRequestHandler<GetInventoryByProductIdQuery, InventoryItemDto>
{
    private readonly IInventoryRepository _inventoryRepository;

    public GetInventoryByProductIdQueryHandler(IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }

    public async Task<InventoryItemDto> Handle(
        GetInventoryByProductIdQuery request,
        CancellationToken cancellationToken)
    {
        var inventoryItem = await _inventoryRepository.GetByProductIdAsync(
            request.ProductId,
            cancellationToken);

        if (inventoryItem is null)
        {
            throw new InventoryItemNotFoundException(request.ProductId);
        }

        return new InventoryItemDto(
            inventoryItem.Id,
            inventoryItem.ProductId,
            inventoryItem.AvailableQuantity,
            inventoryItem.ReservedQuantity,
            inventoryItem.CreatedAt,
            inventoryItem.UpdatedAt);
    }
}
