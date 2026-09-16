using FluentValidation;

namespace OrderManagement.Modules.Products.Application.Features.Products.GetAll;

internal sealed class GetProductsQueryValidator
    : AbstractValidator<GetProductsQuery>
{
    private static readonly string[] AllowedSortFields =
    [
        "name",
        "price",
        "createdat"
    ];

    private static readonly string[] AllowedSortDirections =
    [
         "asc",
         "desc"
    ];

    public GetProductsQueryValidator()
    {
        RuleFor(x => x.Parameters.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than zero.");

        RuleFor(x => x.Parameters.PageSize)
            .InclusiveBetween(1, 50)
            .WithMessage("Page size must be between 1 and 50.");

        RuleFor(x => x.Parameters.SortBy)
            .Must(sortBy =>
                string.IsNullOrWhiteSpace(sortBy) ||
                AllowedSortFields.Contains(
                    sortBy.ToLowerInvariant()))
            .WithMessage(
                "Sort field must be one of: name, price, createdAt.");

        RuleFor(x => x.Parameters.SortDirection)
            .Must(direction =>
                string.IsNullOrWhiteSpace(direction) ||
                AllowedSortDirections.Contains(
                    direction.ToLowerInvariant()))
            .WithMessage(
                "Sort direction must be either 'asc' or 'desc'.");
    }
}
