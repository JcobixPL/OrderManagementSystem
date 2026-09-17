using MediatR;

namespace OrderManagement.Modules.Products.Application.Features.Products.Deactivate;

public sealed record DeactivateProductCommand(Guid ProductId) : IRequest;
