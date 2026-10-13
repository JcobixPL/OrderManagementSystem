namespace OrderManagement.Modules.Payments.Application.DTOs;

public sealed record PaymentDto(
    Guid Id,
    Guid OrderId,
    decimal Amount,
    string Currency,
    string Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? PaidAt,
    IReadOnlyCollection<PaymentAttemptDto> Attempts);

public sealed record PaymentAttemptDto(
    Guid Id,
    string Status,
    string? ExternalPaymentId,
    string? FailureReason,
    DateTimeOffset CreatedAt,
    DateTimeOffset? CompletedAt);