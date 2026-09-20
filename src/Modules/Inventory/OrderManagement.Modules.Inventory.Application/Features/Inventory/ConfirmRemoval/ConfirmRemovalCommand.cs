using MediatR;

namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.ConfirmRemoval;

public sealed record ConfirmRemovalCommand(
    Guid ProductId,
    int Quantity) : IRequest;