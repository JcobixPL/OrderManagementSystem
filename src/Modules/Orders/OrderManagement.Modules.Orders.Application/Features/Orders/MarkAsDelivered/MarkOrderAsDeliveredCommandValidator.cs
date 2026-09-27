using FluentValidation;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.MarkAsDelivered;

internal sealed class MarkOrderAsDeliveredCommandValidator
    : AbstractValidator<MarkOrderAsDeliveredCommand>
{
    public MarkOrderAsDeliveredCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("Order ID cannot be empty.");
    }
}