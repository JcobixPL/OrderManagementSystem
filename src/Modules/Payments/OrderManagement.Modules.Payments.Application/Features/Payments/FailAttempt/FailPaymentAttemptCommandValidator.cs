using FluentValidation;

namespace OrderManagement.Modules.Payments.Application.Features.Payments.FailAttempt;

internal sealed class FailPaymentAttemptCommandValidator
    : AbstractValidator<FailPaymentAttemptCommand>
{
    public FailPaymentAttemptCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty()
            .WithMessage("Payment ID is required.");

        RuleFor(x => x.AttemptId)
            .NotEmpty()
            .WithMessage("Payment attempt ID is required.");

        RuleFor(x => x.FailureReason)
            .NotEmpty()
            .WithMessage("Failure reason is required.");
    }
}