namespace OrderManagement.Modules.Payments.Application.Abstractions;

public interface IOrderPaymentDetailsProvider
{
    Task<OrderPaymentDetails?> GetAsync(
        Guid orderId,
        CancellationToken cancellationToken = default);
}

public sealed record OrderPaymentDetails(
    Guid OrderId,
    Guid CustomerId,
    decimal TotalAmount,
    string Currency,
    string Status);