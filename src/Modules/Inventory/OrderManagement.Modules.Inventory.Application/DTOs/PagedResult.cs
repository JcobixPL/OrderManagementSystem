namespace OrderManagement.Modules.Inventory.Application.DTOs;

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int PageNumber,
    int PageSize,
    int TotalRecords,
    int TotalPages)
{
    public bool HasNextPage => PageNumber < TotalPages;

    public bool HasPreviousPage => PageNumber > 1;
}