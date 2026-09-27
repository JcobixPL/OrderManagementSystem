using MediatR;
using OrderManagement.Modules.Orders.Application.Abstractions;
using OrderManagement.Modules.Orders.Application.Exceptions;
using OrderManagement.Modules.Orders.Domain.Entities;

namespace OrderManagement.Modules.Orders.Application.Features.Orders.Create;

internal sealed class CreateOrderCommandHandler
    : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IProductCatalog _productCatalog;
    private readonly IInventoryService _inventoryService;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrdersUnitOfWork _unitOfWork;
    private readonly IOrdersTransaction _transaction;

    public CreateOrderCommandHandler(
        IProductCatalog productCatalog, 
        IInventoryService inventoryService, 
        IOrderRepository orderRepository, 
        IOrdersUnitOfWork ordersUnitOfWork,
        IOrdersTransaction ordersTransactions)
    {
        _productCatalog = productCatalog;
        _inventoryService = inventoryService;
        _orderRepository = orderRepository;
        _unitOfWork = ordersUnitOfWork;
        _transaction = ordersTransactions;
    }

    public async Task<Guid> Handle(
        CreateOrderCommand request,
        CancellationToken cancellationToken)
    {
        var resolvedItems = new List<ResolvedOrderItem>();

        foreach (var item in request.Items)
        {
            var product = await _productCatalog.GetByIdAsync(
                item.ProductId,
                cancellationToken);

            if (product is null)
                throw new OrderProductNotFoundException(item.ProductId);

            if (!product.IsActive)
                throw new InactiveOrderProductException(item.ProductId);


            resolvedItems.Add(
                new ResolvedOrderItem(
                    product,
                    item.Quantity));
        }

        var firstProduct = resolvedItems[0].Product;

        var order = new Order(
            request.CustomerId,
            firstProduct.Currency);

        foreach (var item in resolvedItems)
        {
            order.AddItem(
                item.Product.Id,
                item.Product.Sku,
                item.Product.Name,
                item.Product.Price,
                item.Product.Currency,
                item.Quantity);
        }

        return await _transaction.ExecuteAsync(
            async transactionCancellationToken =>
            {
                foreach (var item in resolvedItems)
                {
                    await _inventoryService.ReserveAsync(
                        item.Product.Id,
                        item.Quantity,
                        transactionCancellationToken);
                }

                await _orderRepository.AddAsync(
                    order,
                    transactionCancellationToken);

                await _unitOfWork.SaveChangesAsync(
                    transactionCancellationToken);

                return order.Id;
            },
            cancellationToken);
    }

    private sealed record ResolvedOrderItem(
        ProductSnapshot Product,
        int Quantity);
}
