using MediatR;
using OrderManagement.Modules.Orders.Application.Abstractions;
using OrderManagement.Modules.Products.Application.Features.Products.GetById;

namespace OrderManagement.Modules.Orders.Infrastructure.Integrations;

internal sealed class ProductCatalog : IProductCatalog
{
    private readonly ISender _sender;

    public ProductCatalog(ISender sender)
    {
        _sender = sender;
    }

    public async Task<ProductSnapshot?> GetByIdAsync(
        Guid productId, 
        CancellationToken cancellationToken = default)
    {
        var product = await _sender.Send(
            new GetProductByIdQuery(productId),
            cancellationToken);

        if (product is null)
            return null;

        return new ProductSnapshot(
            product.Id,
            product.Sku,
            product.Name,
            product.Price,
            product.Currency,
            product.IsActive);
    }
}
