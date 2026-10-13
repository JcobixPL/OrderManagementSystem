namespace OrderManagement.Modules.Orders.Application.Features.Orders.GetPaymentDetails;

public sealed record OrderPaymentDetailsDto(
    Guid OrderId,
    Guid CustomerId,
    decimal TotalAmount,
    string Currency,
    string Status);