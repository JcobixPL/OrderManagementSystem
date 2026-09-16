using MediatR;
using OrderManagement.Modules.Products.Application.Abstractions;
using OrderManagement.Modules.Products.Application.Exceptions;
using OrderManagement.Modules.Products.Domain.Entities;

namespace OrderManagement.Modules.Products.Application.Features.Products.Create;

internal sealed class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductsUnitOfWork _unitOfWork;

    public CreateProductCommandHandler(IProductRepository productRepository, IProductsUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        if (await _productRepository.ExistsBySkuAsync(request.Sku, cancellationToken))
            throw new DuplicateProductSkuException(request.Sku);

        var price = Money.Create(
            request.Price,
            request.Currency);

        var product = new Product(
            request.Sku,
            request.Name,
            request.Description,
            price);

        await _productRepository.AddAsync(product, cancellationToken);    
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return product.Id;
    }
}
