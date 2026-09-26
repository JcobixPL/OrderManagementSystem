namespace OrderManagement.Modules.Orders.Application.Abstractions;

public interface IOrdersUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
