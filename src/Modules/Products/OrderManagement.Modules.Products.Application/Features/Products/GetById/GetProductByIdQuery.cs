using MediatR;
using OrderManagement.Modules.Products.Application.DTOs;

namespace OrderManagement.Modules.Products.Application.Features.Products.GetById;

public sealed record GetProductByIdQuery(Guid id) : IRequest<ProductDto?>;
