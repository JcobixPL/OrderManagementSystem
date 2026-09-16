namespace OrderManagement.Modules.Products.Application.Exceptions;

public sealed class DuplicateProductSkuException : Exception
{
    public DuplicateProductSkuException(string sku)
        : base($"Product with SKU {sku} already exists.")
    {
        Sku = sku;
    }

    public string Sku { get; }
}
