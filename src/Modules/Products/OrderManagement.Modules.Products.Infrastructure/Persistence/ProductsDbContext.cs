using Microsoft.EntityFrameworkCore;
using OrderManagement.Modules.Products.Domain.Entities;

namespace OrderManagement.Modules.Products.Infrastructure.Persistence;

public sealed class ProductsDbContext : DbContext
{
    public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("products");

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ProductsDbContext).Assembly
        );
    }
}
