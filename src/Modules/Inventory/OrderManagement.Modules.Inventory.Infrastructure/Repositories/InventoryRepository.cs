using Microsoft.EntityFrameworkCore;
using OrderManagement.Modules.Inventory.Application.Abstractions;
using OrderManagement.Modules.Inventory.Domain.Entities;
using OrderManagement.Modules.Inventory.Infrastructure.Persistence;

namespace OrderManagement.Modules.Inventory.Infrastructure.Repositories;

internal sealed class InventoryRepository : IInventoryRepository
{
    private readonly InventoryDbContext _dbContext;

    public InventoryRepository(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<InventoryItem?> GetByProductIdAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.InventoryItems
            .SingleOrDefaultAsync(
                x => x.ProductId == productId,
                cancellationToken);
    }

    public async Task AddAsync(
        InventoryItem inventoryItem,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.InventoryItems.AddAsync(
            inventoryItem, cancellationToken);
    }

    public Task<bool> ExistsByProductIdAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.InventoryItems.AnyAsync(
            x => x.ProductId == productId,
            cancellationToken);
    }
}
