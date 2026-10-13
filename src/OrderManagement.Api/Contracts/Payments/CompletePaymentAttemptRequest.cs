namespace OrderManagement.Api.Contracts.Payments;

public sealed record CompletePaymentAttemptRequest(
    string ExternalPaymentId);