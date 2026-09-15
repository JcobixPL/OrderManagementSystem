using Microsoft.EntityFrameworkCore;
using OrderManagement.Modules.Products.Application.Abstractions;
using OrderManagement.Modules.Products.Domain.Entities;
using OrderManagement.Modules.Products.Infrastructure.Persistence;

namespace OrderManagement.Modules.Products.Infrastructure.Repositories;

internal sealed class ProductRepository : IProductRepository
{
    private readonly ProductsDbContext _dbContext;

    public ProductRepository(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _dbContext.Products.AddAsync(product, cancellationToken);
    }

    public async Task<bool> ExistsBySkuAsync(string sku, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products.AnyAsync(x => x.Sku == sku, cancellationToken);
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
