using MediatR;

namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.AddStock;

public sealed record AddStockCommand(
    Guid ProductId,
    int Quantity) : IRequest;
