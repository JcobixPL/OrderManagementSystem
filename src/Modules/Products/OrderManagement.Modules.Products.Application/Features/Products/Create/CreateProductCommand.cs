using MediatR;

namespace OrderManagement.Modules.Products.Application.Features.Products.Create;

public sealed record CreateProductCommand(
    string Sku,
    string Name,
    string Description,
    decimal Price,
    string Currency) : IRequest<Guid>;
