using FluentValidation;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.Cancel;

internal sealed class CancelOrderCommandValidator
    : AbstractValidator<CancelOrderCommand>
{
    public CancelOrderCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("Order ID cannot be empty.");
    }
}