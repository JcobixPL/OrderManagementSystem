using FluentValidation;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.MarkAsPaid;

internal sealed class MarkOrderAsPaidCommandValidator
    : AbstractValidator<MarkOrderAsPaidCommand>
{
    public MarkOrderAsPaidCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("Order ID cannot be empty.");
    }
}
