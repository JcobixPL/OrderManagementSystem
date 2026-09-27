using FluentValidation;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.MarkReadyForShipment;

internal sealed class MarkOrderReadyForShipmentCommandValidator
    : AbstractValidator<MarkOrderReadyForShipmentCommand>
{
    public MarkOrderReadyForShipmentCommandValidator()
    {
        RuleFor(x => x.OrderId)
            .NotEmpty()
            .WithMessage("Order ID cannot be empty.");
    }
}