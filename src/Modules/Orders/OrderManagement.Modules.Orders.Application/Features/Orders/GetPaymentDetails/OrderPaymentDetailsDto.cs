namespace OrderManagement.Modules.Orders.Application.Features.Orders.GetPaymentDetails;

public sealed record OrderPaymentDetailsDto(
    Guid OrderId,
    decimal TotalAmount,
    string Currency,
    string Status);