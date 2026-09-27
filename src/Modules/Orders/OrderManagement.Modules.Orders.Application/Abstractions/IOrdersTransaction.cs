namespace OrderManagement.Modules.Orders.Application.Abstractions;

public interface IOrdersTransaction
{
    Task<T> ExecuteAsync<T>(
        Func<CancellationToken, Task<T>> operation,
        CancellationToken cancellationToken = default);
}
