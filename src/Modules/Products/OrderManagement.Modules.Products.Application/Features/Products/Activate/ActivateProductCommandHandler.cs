using MediatR;
using OrderManagement.Modules.Products.Application.Abstractions;
using OrderManagement.Modules.Products.Application.Exceptions;

namespace OrderManagement.Modules.Products.Application.Features.Products.Activate;

internal sealed class ActivateProductCommandHandler : IRequestHandler<ActivateProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductsUnitOfWork _unitOfWork;

    public ActivateProductCommandHandler(IProductRepository productRepository, IProductsUnitOfWork productsUnitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = productsUnitOfWork;
    }

    public async Task Handle(
        ActivateProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId);

        if (product is null)
            throw new ProductNotFoundException(request.ProductId);

        if (product.IsActive)
            throw new ProductAlreadyActiveException(request.ProductId);

        product.Activate();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
