using Inventory.Domain.Enums;
using Inventory.Domain.Exceptions;

namespace Inventory.Domain.Entities;

public sealed class StockMovement
{
    private StockMovement()
    {
    }

    public StockMovement(int productId, int quantity, StockMovementType type)
    {
        if (productId <= 0)
        {
            throw new DomainException("El producto del movimiento es obligatorio.");
        }

        if (quantity <= 0)
        {
            throw new DomainException("La cantidad del movimiento debe ser mayor a cero.");
        }

        ProductId = productId;
        Quantity = quantity;
        Type = type;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public int Id { get; private set; }
    public int ProductId { get; private set; }
    public Product? Product { get; private set; }
    public int Quantity { get; private set; }
    public StockMovementType Type { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
}
