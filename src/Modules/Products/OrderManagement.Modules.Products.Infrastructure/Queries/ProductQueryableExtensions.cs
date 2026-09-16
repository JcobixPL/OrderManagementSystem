using Microsoft.EntityFrameworkCore;
using OrderManagement.Modules.Products.Domain.Entities;

namespace OrderManagement.Modules.Products.Infrastructure.Queries;

internal static class ProductQueryableExtensions
{
    public static IQueryable<Product> ApplySearch(
        this IQueryable<Product> query,
        string? search)
    {
        if (string.IsNullOrWhiteSpace(search))
            return query;

        var pattern = $"%{search.Trim()}%";

        return query.Where(product =>
            EF.Functions.ILike(product.Name, pattern) ||
            EF.Functions.ILike(product.Sku, pattern));
    }

    public static IQueryable<Product> ApplyActiveFilter(
        this IQueryable<Product> query,
        bool? isActive)
    {
        if (!isActive.HasValue)
            return query;

        return query.Where(product =>
            product.IsActive == isActive.Value);
    }

    public static IQueryable<Product> ApplySorting(
        this IQueryable<Product> query,
        string? sortBy,
        string? sortDirection)
    {
        var descending =
            sortDirection?.Equals("desc", StringComparison.OrdinalIgnoreCase) == true;

        return sortBy?.ToLowerInvariant() switch
        {
            "name" => descending
                ? query.OrderByDescending(product => product.Name)
                : query.OrderBy(product => product.Name),

            "price" => descending
                ? query.OrderByDescending(product => product.Price.Amount)
                : query.OrderBy(product => product.Price.Amount),

            "createdat" => descending
                ? query.OrderByDescending(product => product.CreatedAt)
                : query.OrderBy(product => product.CreatedAt),

            _ => query.OrderByDescending(product => product.CreatedAt)
        };
    }

    public static IQueryable<Product> ApplyPagination(
        this IQueryable<Product> query,
        int pageNumber,
        int pageSize)
    {
        return query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
    }
}
