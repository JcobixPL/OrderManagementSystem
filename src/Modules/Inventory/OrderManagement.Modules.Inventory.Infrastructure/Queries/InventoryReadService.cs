using Microsoft.EntityFrameworkCore;
using OrderManagement.Modules.Inventory.Application.Abstractions;
using OrderManagement.Modules.Inventory.Application.DTOs;
using OrderManagement.Modules.Inventory.Application.Features.Inventory.GetAll;
using OrderManagement.Modules.Inventory.Infrastructure.Persistence;

namespace OrderManagement.Modules.Inventory.Infrastructure.Queries;

internal sealed class InventoryReadService : IInventoryReadService
{
    private readonly InventoryDbContext _dbContext;

    public InventoryReadService(InventoryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PagedResult<InventoryItemDto>> GetPagedAsync(
        InventoryQueryParameters parameters,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.InventoryItems
            .AsNoTracking()
            .AsQueryable();

        if (parameters.HasAvailableStock.HasValue)
        {
            query = parameters.HasAvailableStock.Value
                ? query.Where(x => x.AvailableQuantity > 0)
                : query.Where(x => x.AvailableQuantity == 0);
        }

        if (parameters.HasReservedStock.HasValue)
        {
            query = parameters.HasReservedStock.Value
                ? query.Where(x => x.ReservedQuantity > 0)
                : query.Where(x => x.ReservedQuantity == 0);
        }

        var totalRecords = await query.CountAsync(cancellationToken);

        query = parameters.SortBy?.ToLowerInvariant() switch
        {
            "availablequantity" => parameters.SortDirection?.ToLowerInvariant() == "desc"
                ? query.OrderByDescending(x => x.AvailableQuantity)
                : query.OrderBy(x => x.AvailableQuantity),

            "reservedquantity" => parameters.SortDirection?.ToLowerInvariant() == "desc"
                ? query.OrderByDescending(x => x.ReservedQuantity)
                : query.OrderBy(x => x.ReservedQuantity),

            "createdat" => parameters.SortDirection?.ToLowerInvariant() == "asc"
                ? query.OrderBy(x => x.CreatedAt)
                : query.OrderByDescending(x => x.CreatedAt),

            _ => query.OrderByDescending(x => x.CreatedAt)
        };

        var items = await query
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .Select(x => new InventoryItemDto(
                x.Id,
                x.ProductId,
                x.AvailableQuantity,
                x.ReservedQuantity,
                x.CreatedAt,
                x.UpdatedAt))
            .ToListAsync(cancellationToken);

        var totalPages = (int)Math.Ceiling(
            totalRecords / (double)parameters.PageSize);

        return new PagedResult<InventoryItemDto>(
            items,
            parameters.PageNumber,
            parameters.PageSize,
            totalRecords,
            totalPages);
    }
}