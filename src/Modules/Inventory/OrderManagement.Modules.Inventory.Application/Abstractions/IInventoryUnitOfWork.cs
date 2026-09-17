namespace OrderManagement.Modules.Inventory.Application.Abstractions;

public interface IInventoryUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
