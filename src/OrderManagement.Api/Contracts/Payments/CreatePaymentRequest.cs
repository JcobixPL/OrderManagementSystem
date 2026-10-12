namespace OrderManagement.Api.Contracts.Payments;

public sealed record CreatePaymentRequest(Guid OrderId);