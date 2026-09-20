using MediatR;

namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.Create;

public sealed record CreateInventoryItemCommand(
    Guid ProductId,
    int InitialQuantity) : IRequest<Guid>;
