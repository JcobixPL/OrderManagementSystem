using MediatR;
using OrderManagement.Modules.Products.Application.DTOs;

namespace OrderManagement.Modules.Products.Application.Features.Products.GetAll;

public sealed record GetProductsQuery
    (ProductQueryParameters Parameters) : IRequest<PagedResult<ProductDto>>;