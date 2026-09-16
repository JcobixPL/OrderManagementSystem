using MediatR;
using OrderManagement.Modules.Products.Application.Abstractions;
using OrderManagement.Modules.Products.Application.DTOs;

namespace OrderManagement.Modules.Products.Application.Features.Products.GetAll;

internal sealed class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResult<ProductDto>>
{
    private readonly IProductReadService _productReadService;

    public GetProductsQueryHandler(IProductReadService productReadService)
    {
        _productReadService = productReadService;
    }

    public Task<PagedResult<ProductDto>> Handle(
        GetProductsQuery request,
        CancellationToken cancellationToken)
    {
        return _productReadService.GetPagedAsync(request.Parameters, cancellationToken);
    }
}
