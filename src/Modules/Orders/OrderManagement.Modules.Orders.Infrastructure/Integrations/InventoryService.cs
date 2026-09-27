using MediatR;
using OrderManagement.Modules.Inventory.Application.Features.Inventory.ConfirmRemoval;
using OrderManagement.Modules.Inventory.Application.Features.Inventory.ReleaseReservation;
using OrderManagement.Modules.Inventory.Application.Features.Inventory.Reserve;
using OrderManagement.Modules.Orders.Application.Abstractions;

namespace OrderManagement.Modules.Orders.Infrastructure.Integrations;

internal sealed class InventoryService : IInventoryService
{
    private readonly ISender _sender;

    public InventoryService(ISender sender)
    {
        _sender = sender;
    }

    public Task ReleaseReservationAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default)
    {
        return _sender.Send(
            new ReleaseReservationCommand(productId, quantity),
            cancellationToken);
    }

    public Task ReserveAsync(
        Guid productId, 
        int quantity, 
        CancellationToken cancellationToken = default)
    {
        return _sender.Send(
            new ReserveStockCommand(productId, quantity),
            cancellationToken);
    }

    public Task ConfirmRemovalAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default)
    {
        return _sender.Send(
            new ConfirmRemovalCommand(productId, quantity),
            cancellationToken);
    }
}
