using Microsoft.EntityFrameworkCore;
using OrderManagement.Modules.Orders.Application.Abstractions;
using OrderManagement.Modules.Orders.Domain.Entities;
using OrderManagement.Modules.Orders.Infrastructure.Persistence;

namespace OrderManagement.Modules.Orders.Infrastructure.Repositories;

internal sealed class OrderRepository : IOrderRepository
{
    private readonly OrdersDbContext _dbContext;

    public OrderRepository(OrdersDbContext ordersDbContext)
    {
        _dbContext = ordersDbContext;
    }

    public Task<Order?> GetByIdAsync(
        Guid orderId, 
        CancellationToken cancellationToken = default)
    {
        return _dbContext.Orders
            .Include(x => x.Items)
            .SingleOrDefaultAsync(
                x => x.Id == orderId,
                cancellationToken);
    }

    public async Task AddAsync(
        Order order,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Orders.AddAsync(
            order,
            cancellationToken);
    }
}
