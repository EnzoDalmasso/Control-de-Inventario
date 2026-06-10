using Inventory.Domain.Entities;

namespace Inventory.Api.Contracts.StockMovements;

public sealed record StockMovementResponse(
    int Id,
    int ProductId,
    string ProductName,
    int Quantity,
    string Type,
    DateTime CreatedAtUtc)
{
    public static StockMovementResponse From(StockMovement movement)
    {
        return new StockMovementResponse(
            movement.Id,
            movement.ProductId,
            movement.Product?.Name ?? string.Empty,
            movement.Quantity,
            movement.Type.ToString(),
            movement.CreatedAtUtc);
    }
}
