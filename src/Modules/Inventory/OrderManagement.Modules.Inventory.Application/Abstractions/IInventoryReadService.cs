using OrderManagement.Modules.Inventory.Application.DTOs;
using OrderManagement.Modules.Inventory.Application.Features.Inventory.GetAll;

namespace OrderManagement.Modules.Inventory.Application.Abstractions;

public interface IInventoryReadService
{
    Task<PagedResult<InventoryItemDto>> GetPagedAsync(
        InventoryQueryParameters parameters,
        CancellationToken cancellationToken = default);
}
