using FluentValidation;

namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.ConfirmRemoval;

internal sealed class ConfirmRemovalCommandValidator
    : AbstractValidator<ConfirmRemovalCommand>
{
    public ConfirmRemovalCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID cannot be empty.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}