using FluentValidation;

namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.AddStock;

internal sealed class AddStockCommandValidator
    : AbstractValidator<AddStockCommand>
{
    public AddStockCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID cannot be empty.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}
