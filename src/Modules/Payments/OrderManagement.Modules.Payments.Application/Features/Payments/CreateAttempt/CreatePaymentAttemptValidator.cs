using FluentValidation;

namespace OrderManagement.Modules.Payments.Application.Features.Payments.CreateAttempt;

internal sealed class CreatePaymentAttemptCommandValidator
    : AbstractValidator<CreatePaymentAttemptCommand>
{
    public CreatePaymentAttemptCommandValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty()
            .WithMessage("Payment ID is required.");
    }
}