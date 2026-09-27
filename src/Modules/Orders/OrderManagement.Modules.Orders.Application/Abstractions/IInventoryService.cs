namespace OrderManagement.Modules.Orders.Application.Abstractions;

public interface IInventoryService
{
    Task ReserveAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default);

    Task ReleaseReservationAsync(
        Guid productId,
        int quantity, 
        CancellationToken cancellationToken = default);

    Task ConfirmRemovalAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken = default);
}
