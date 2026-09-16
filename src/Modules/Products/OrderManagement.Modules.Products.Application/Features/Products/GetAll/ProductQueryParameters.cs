namespace OrderManagement.Modules.Products.Application.Features.Products.GetAll;

public sealed record ProductQueryParameters(
    int PageNumber = 1,
    int PageSize = 10,
    string? Search = null,
    bool? IsActive = null,
    string? SortBy = null,
    string? SortDirection = null);
