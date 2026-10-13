using FluentValidation;

namespace OrderManagement.Modules.Payments.Application.Features.Payments.CompleteAttempt;

internal sealed class CompletePaymentAttemptCommandValidator
    : AbstractValidator<CompletePaymentAttemptCommand>
{
    public CompletePaymentAttemptCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty()
            .WithMessage("Payment ID is required.");

        RuleFor(x => x.AttemptId)
            .NotEmpty()
            .WithMessage("Payment attempt ID is required.");

        RuleFor(x => x.ExternalPaymentId)
            .NotEmpty()
            .WithMessage("External payment ID is required.");
    }
}