namespace OrderManagement.Modules.Inventory.Application.DTOs;

public sealed record InventoryItemDto(
    Guid Id,
    Guid ProductId,
    int AvailableQuantity,
    int ReservedQuantity,
    DateTimeOffset CreatedAt,
    DateTimeOffset? UpdatedAt);
