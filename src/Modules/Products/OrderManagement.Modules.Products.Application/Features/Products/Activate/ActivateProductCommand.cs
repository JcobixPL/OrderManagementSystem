using MediatR;

namespace OrderManagement.Modules.Products.Application.Features.Products.Activate;

public sealed record ActivateProductCommand(Guid ProductId) : IRequest;
