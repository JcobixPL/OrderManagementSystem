using FluentValidation;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.MarkAsShipped;

internal sealed class MarkOrderAsShippedCommandValidator
    : AbstractValidator<MarkOrderAsShippedCommand>
{
    public MarkOrderAsShippedCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("Order ID cannot be empty.");
    }
}