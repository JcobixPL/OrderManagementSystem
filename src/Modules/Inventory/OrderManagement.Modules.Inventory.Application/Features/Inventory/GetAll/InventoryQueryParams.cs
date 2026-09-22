namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.GetAll;

public sealed record InventoryQueryParameters(
    int PageNumber = 1,
    int PageSize = 10,
    bool? HasAvailableStock = null,
    bool? HasReservedStock = null,
    string? SortBy = null,
    string? SortDirection = null);