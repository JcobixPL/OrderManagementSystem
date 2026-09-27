using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OrderManagement.Modules.Inventory.Infrastructure.Persistence;
using OrderManagement.Modules.Orders.Application.Abstractions;
using OrderManagement.Modules.Orders.Infrastructure.Persistence;

namespace OrderManagement.Api.Infrastructure.Transactions;

internal sealed class OrdersTransaction : IOrdersTransaction
{
    private readonly OrdersDbContext _ordersDbContext;
    private readonly InventoryDbContext _inventoryDbContext;

    public OrdersTransaction(OrdersDbContext ordersDbContext, InventoryDbContext inventoryDbContext)
    {
        _ordersDbContext = ordersDbContext;
        _inventoryDbContext = inventoryDbContext;
    }

    public async Task<T> ExecuteAsync<T>(
        Func<CancellationToken, Task<T>> operation,
        CancellationToken cancellationToken = default)
    {
        await using var transaction =
            await _ordersDbContext.Database.BeginTransactionAsync(cancellationToken);

        await _inventoryDbContext.Database.UseTransactionAsync(
            transaction.GetDbTransaction(),
            cancellationToken);

        try
        {
            var result = await operation(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return result;
        }
        catch
        {
            await transaction.RollbackAsync(CancellationToken.None);
            throw;
        }
    }
}
