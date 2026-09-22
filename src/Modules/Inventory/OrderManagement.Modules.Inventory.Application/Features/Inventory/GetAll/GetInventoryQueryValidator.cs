using FluentValidation;

namespace OrderManagement.Modules.Inventory.Application.Features.Inventory.GetAll;

internal sealed class GetInventoryQueryValidator
    : AbstractValidator<GetInventoryQuery>
{
    private static readonly string[] AllowedSortFields =
    [
        "availableQuantity",
        "reservedQuantity",
        "createdAt"
    ];

    private static readonly string[] AllowedSortDirections =
    [
        "asc",
        "desc"
    ];

    public GetInventoryQueryValidator()
    {
        RuleFor(x => x.Parameters.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than zero.");

        RuleFor(x => x.Parameters.PageSize)
            .InclusiveBetween(1, 50)
            .WithMessage("Page size must be between 1 and 50.");

        RuleFor(x => x.Parameters.SortBy)
            .Must(sortBy =>
                sortBy is null ||
                AllowedSortFields.Contains(
                    sortBy,
                    StringComparer.OrdinalIgnoreCase))
            .WithMessage(
                "Sort by must be one of: availableQuantity, reservedQuantity, createdAt.");

        RuleFor(x => x.Parameters.SortDirection)
            .Must(direction =>
                direction is null ||
                AllowedSortDirections.Contains(
                    direction,
                    StringComparer.OrdinalIgnoreCase))
            .WithMessage(
                "Sort direction must be either asc or desc.");
    }
}