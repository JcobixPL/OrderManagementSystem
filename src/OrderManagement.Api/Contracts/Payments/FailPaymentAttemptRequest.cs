namespace OrderManagement.Api.Contracts.Payments;

public sealed record FailPaymentAttemptRequest(
    string FailureReason);