namespace OrderManagement.Api.Contracts.Inventory;

public sealed record CreateInventoryItemRequest(
    Guid ProductId,
    int InitialQuantity);
