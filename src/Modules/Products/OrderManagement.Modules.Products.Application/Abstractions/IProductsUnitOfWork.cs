namespace OrderManagement.Modules.Products.Application.Abstractions;

public interface IProductsUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
