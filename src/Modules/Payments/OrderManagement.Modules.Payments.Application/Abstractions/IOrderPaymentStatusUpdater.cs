namespace OrderManagement.Modules.Payments.Application.Abstractions;

public interface IOrderPaymentStatusUpdater
{
    Task MarkAsPaidAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);
}