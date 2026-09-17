using MediatR;
using OrderManagement.Modules.Products.Application.Abstractions;
using OrderManagement.Modules.Products.Application.Exceptions;
using OrderManagement.Modules.Products.Domain.Entities;

namespace OrderManagement.Modules.Products.Application.Features.Products.Rename;

internal sealed class RenameProductCommandHandler : IRequestHandler<RenameProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductsUnitOfWork _unitOfWork;

    public RenameProductCommandHandler(IProductRepository productRepository, IProductsUnitOfWork productsUnitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = productsUnitOfWork;
    }

    public async Task Handle(
        RenameProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken);

        if (product is null)
            throw new ProductNotFoundException(request.ProductId);

        product.Rename(request.Name);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
