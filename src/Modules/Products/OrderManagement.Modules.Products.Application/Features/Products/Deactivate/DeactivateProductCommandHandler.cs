using MediatR;
using OrderManagement.Modules.Products.Application.Abstractions;
using OrderManagement.Modules.Products.Application.Exceptions;

namespace OrderManagement.Modules.Products.Application.Features.Products.Deactivate;

internal sealed class DeactivateProductCommandHandler : IRequestHandler<DeactivateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductsUnitOfWork _unitOfWork;

    public DeactivateProductCommandHandler(IProductRepository productRepository, IProductsUnitOfWork productsUnitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = productsUnitOfWork;
    }

    public async Task Handle(
        DeactivateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
            throw new ProductNotFoundException(request.ProductId);

        if (!product.IsActive)
            throw new ProductAlreadyInactiveException(request.ProductId);

        product.Deactivate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
