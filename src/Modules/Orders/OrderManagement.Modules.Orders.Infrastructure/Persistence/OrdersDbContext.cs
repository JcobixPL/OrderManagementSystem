using Microsoft.EntityFrameworkCore;
using OrderManagement.Modules.Orders.Application.Abstractions;
using OrderManagement.Modules.Orders.Domain.Entities;

namespace OrderManagement.Modules.Orders.Infrastructure.Persistence;

internal sealed class OrdersDbContext : DbContext, IOrdersUnitOfWork
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options) { }

    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("products");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(OrdersDbContext).Assembly
            );
    }
}
