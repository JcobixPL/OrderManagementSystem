namespace OrderManagement.Modules.Payments.Application.Abstractions;

public interface IOrderPaymentDetailsProvider
{
    Task<OrderPaymentDetails?> GetAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);
}

public sealed record OrderPaymentDetails(
    Guid OrderId,
    decimal TotalAmount,
    string Currency,
    string Status);