using OrderManagement.Modules.Orders.Application.Abstractions;

namespace OrderManagement.Modules.Orders.UnitTests;

internal sealed class TestOrdersTransaction : IOrdersTransaction
{
    public async Task<T> ExecuteAsync<T>(
        Func<CancellationToken, Task<T>> operation,
        CancellationToken cancellationToken = default)
    {
        return await operation(cancellationToken);
    }
}
