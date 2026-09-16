namespace OrderManagement.Modules.Products.Application.DTOs;

public sealed record ProductDto(
    Guid Id,
    string Sku,
    string Name,
    string Description,
    decimal Price,
    string Currency,
    bool IsActive,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);
