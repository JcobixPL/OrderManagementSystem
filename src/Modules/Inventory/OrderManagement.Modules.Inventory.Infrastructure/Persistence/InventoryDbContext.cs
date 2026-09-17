using Microsoft.EntityFrameworkCore;
using OrderManagement.Modules.Inventory.Application.Abstractions;
using OrderManagement.Modules.Inventory.Domain.Entities;

namespace OrderManagement.Modules.Inventory.Infrastructure.Persistence;

public sealed class InventoryDbContext : DbContext, IInventoryUnitOfWork
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    this_will_not_compile
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("inventory");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(InventoryDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
