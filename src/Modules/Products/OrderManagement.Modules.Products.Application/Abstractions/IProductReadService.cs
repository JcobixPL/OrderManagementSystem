using OrderManagement.Modules.Products.Application.DTOs;
using OrderManagement.Modules.Products.Application.Features.Products.GetAll;

namespace OrderManagement.Modules.Products.Application.Abstractions;

public interface IProductReadService
{
    Task<PagedResult<ProductDto>> GetPagedAsync(
        ProductQueryParameters parameters,
        CancellationToken cancellationToken = default);
}
