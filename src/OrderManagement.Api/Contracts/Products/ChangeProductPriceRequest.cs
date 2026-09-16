namespace OrderManagement.Api.Contracts.Products;

public sealed record ChangeProductPriceRequest(
    decimal Price,
    string Currency);
