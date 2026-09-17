using MediatR;

namespace OrderManagement.Modules.Products.Application.Features.Products.Rename;

public sealed record RenameProductCommand(
    Guid ProductId,
    string Name) : IRequest;