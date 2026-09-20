using FluentValidation;

namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.ReleaseReservation;

internal sealed class ReleaseReservationCommandValidator
    : AbstractValidator<ReleaseReservationCommand>
{
    public ReleaseReservationCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID cannot be empty.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}