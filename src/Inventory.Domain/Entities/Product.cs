using Inventory.Domain.Exceptions;

namespace Inventory.Domain.Entities;

public sealed class Product
{
    private const int DefaultMinimumStock = 5;

    private Product()
    {
    }

    public Product(string name, int stock, int minimumStock = DefaultMinimumStock)
    {
        ValidateName(name);
        ValidateStock(stock);
        ValidateMinimumStock(minimumStock);

        Name = name.Trim();
        Stock = stock;
        MinimumStock = minimumStock;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int Stock { get; private set; }
    public int MinimumStock { get; private set; } = DefaultMinimumStock;
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? UpdatedAtUtc { get; private set; }

    public bool IsOutOfStock => Stock == 0;
    public bool IsLowStock => Stock > 0 && Stock < MinimumStock;

    public void Rename(string name)
    {
        ValidateName(name);

        Name = name.Trim();
        MarkAsUpdated();
    }

    public void UpdateMinimumStock(int minimumStock)
    {
        ValidateMinimumStock(minimumStock);

        MinimumStock = minimumStock;
        MarkAsUpdated();
    }

    public void IncreaseStock(int quantity)
    {
        ValidateMovementQuantity(quantity);

        Stock += quantity;
        MarkAsUpdated();
    }

    public void DecreaseStock(int quantity)
    {
        ValidateMovementQuantity(quantity);

        if (quantity > Stock)
        {
            throw new DomainException("No hay stock suficiente para realizar el egreso.");
        }

        Stock -= quantity;
        MarkAsUpdated();
    }

    private void MarkAsUpdated()
    {
        UpdatedAtUtc = DateTime.UtcNow;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("El nombre del producto es obligatorio.");
        }

        if (name.Trim().Length > 120)
        {
            throw new DomainException("El nombre del producto no puede superar los 120 caracteres.");
        }
    }

    private static void ValidateStock(int stock)
    {
        if (stock < 0)
        {
            throw new DomainException("El stock inicial no puede ser negativo.");
        }
    }

    private static void ValidateMinimumStock(int minimumStock)
    {
        if (minimumStock < 0)
        {
            throw new DomainException("El stock minimo no puede ser negativo.");
        }
    }

    private static void ValidateMovementQuantity(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("La cantidad del movimiento debe ser mayor a cero.");
        }
    }
}
