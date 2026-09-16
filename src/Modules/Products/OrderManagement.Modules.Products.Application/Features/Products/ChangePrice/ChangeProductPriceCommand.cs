using MediatR;

namespace OrderManagement.Modules.Products.Application.Features.Products.ChangePrice;

public sealed record ChangeProductPriceCommand(
    Guid ProductId,
    decimal Price,
    string Currency) : IRequest;
