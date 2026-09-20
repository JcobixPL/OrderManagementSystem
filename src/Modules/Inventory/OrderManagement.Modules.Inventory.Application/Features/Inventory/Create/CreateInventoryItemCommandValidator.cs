using FluentValidation;

namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.Create;

internal sealed class CreateInventoryItemCommandValidator
    : AbstractValidator<CreateInventoryItemCommand>
{
    public CreateInventoryItemCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product ID cannot be empty.");

        RuleFor(x => x.InitialQuantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Initial quantity must be at least 0 or greater");
    }
}
