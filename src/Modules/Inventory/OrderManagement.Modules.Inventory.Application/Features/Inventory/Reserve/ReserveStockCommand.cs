using MediatR;

namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.Reserve;

public sealed record ReserveStockCommand(
    Guid ProductId,
    int Quantity) : IRequest;
