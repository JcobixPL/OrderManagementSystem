using MediatR;
using OrderManagement.Modules.Products.Application.Abstractions;
using OrderManagement.Modules.Products.Application.Exceptions;
using OrderManagement.Modules.Products.Domain.Entities;

namespace OrderManagement.Modules.Products.Application.Features.Products.ChangePrice;

internal sealed class ChangeProductPriceCommandHandler : IRequestHandler<ChangeProductPriceCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductsUnitOfWork _unitOfWork;

    public ChangeProductPriceCommandHandler(IProductRepository productRepository, IProductsUnitOfWork productsUnitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = productsUnitOfWork;
    }

    public async Task Handle(
        ChangeProductPriceCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(
            request.ProductId,
            cancellationToken);

        if (product is null)
            throw new ProductNotFoundException(request.ProductId);

        product.ChangePrice(
            Money.Create(request.Price, request.Currency));

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
