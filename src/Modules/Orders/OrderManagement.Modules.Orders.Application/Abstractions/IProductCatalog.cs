namespace OrderManagement.Modules.Orders.Application.Abstractions;

public interface IProductCatalog
{
    Task<ProductSnapshot?> GetByIdAsync(
        Guid productId,
        CancellationToken cancellationToken = default);
}

public sealed record ProductSnapshot(
    Guid Id,
    string Sku,
    string Name,
    decimal Price,
    string Currency,
    bool IsActive);
