using System.Xml.Linq;

namespace OrderManagement.Modules.Products.Domain.Entities;
public sealed class Product
{
    public Guid Id { get; private set; }
    public string Sku { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public Money Price { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? UpdatedAt { get; private set; }

    private Product() { }

    public Product(
        string sku,
        string name,
        string description,
        Money price)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("SKU cannot be empty.", nameof(sku));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));

        Id = Guid.NewGuid();
        Sku = sku;
        Name = name;
        Description = description;
        Price = price;
        IsActive = true;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void ChangePrice(Money newPrice)
    {
        Price = newPrice;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
            throw new ArgumentException("Name cannot be empty.", nameof(newName));

        Name = newName;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidOperationException("Product is already inactive.");

        IsActive = false;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Activate()
    {
        if (IsActive)
            throw new InvalidOperationException("Product is already active.");
        
        IsActive = true;
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}