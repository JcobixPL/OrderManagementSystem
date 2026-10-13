using FluentValidation;

namespace OrderManagement.Modules.Payments.Application.Features.Payments.GetById;

internal sealed class GetPaymentByIdQueryValidator
    : AbstractValidator<GetPaymentByIdQuery>
{
    public GetPaymentByIdQueryValidator()
    {
        RuleFor(x => x.PaymentId)
            .NotEmpty()
            .WithMessage("Payment ID is required.");
    }
}