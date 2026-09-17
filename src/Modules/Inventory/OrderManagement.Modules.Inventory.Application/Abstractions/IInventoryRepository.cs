using OrderManagement.Modules.Inventory.Domain.Entities;

namespace OrderManagement.Modules.Inventory.Application.Abstractions;

public interface IInventoryRepository
{
    Task<InventoryItem?> GetByProductIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task AddAsync(InventoryItem inventoryItem, CancellationToken cancellationToken = default);
}
