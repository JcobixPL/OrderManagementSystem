using Microsoft.EntityFrameworkCore;
using OrderManagement.Modules.Products.Application.Abstractions;
using OrderManagement.Modules.Products.Application.DTOs;
using OrderManagement.Modules.Products.Application.Features.Products.GetAll;
using OrderManagement.Modules.Products.Infrastructure.Persistence;

namespace OrderManagement.Modules.Products.Infrastructure.Queries;

internal sealed class ProductReadService : IProductReadService
{
    private const int MaxPageSize = 50;

    private readonly ProductsDbContext _dbContext;

    public ProductReadService(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<ProductDto>> GetPagedAsync(
        ProductQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var pageNumber = parameters.PageNumber;
        var pageSize = parameters.PageSize;

        var query = _dbContext.Products.AsNoTracking().AsQueryable();

        query = query.ApplySearch(parameters.Search);

        query = query.ApplyActiveFilter(parameters.IsActive);

        var totalRecords = await query.CountAsync(cancellationToken);

        query = query.ApplySorting(parameters.SortBy, parameters.SortDirection);

        var items = await query
            .ApplyPagination(pageNumber, pageSize)
            .Select(product => new ProductDto(
                product.Id,
                product.Sku,
                product.Name,
                product.Description,
                product.Price.Amount,
                product.Price.Currency,
                product.IsActive,
                product.CreatedAt,
                product.UpdatedAt))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

        return new PagedResult<ProductDto>(
            items,
            pageNumber,
            pageSize,
            totalRecords,
            totalPages);
    }
}
