using MediatR;
using OrderManagement.Modules.Products.Application.Abstractions;
using OrderManagement.Modules.Products.Application.DTOs;

namespace OrderManagement.Modules.Products.Application.Features.Products.GetById;

internal sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ProductDto?> Handle(
        GetProductByIdQuery request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.id, cancellationToken);

        if (product is null)
            return null;

        return new ProductDto(
            product.Id,
            product.Sku,
            product.Name,
            product.Description,
            product.Price.Amount,
            product.Price.Currency,
            product.IsActive,
            product.CreatedAt,
            product.UpdatedAt);
    }
}
