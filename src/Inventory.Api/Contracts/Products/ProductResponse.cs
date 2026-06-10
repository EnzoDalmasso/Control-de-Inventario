using Inventory.Domain.Entities;

namespace Inventory.Api.Contracts.Products;

public sealed record ProductResponse(
    int Id,
    string Name,
    int Stock,
    int MinimumStock,
    bool IsOutOfStock,
    bool IsLowStock,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc)
{
    public static ProductResponse From(Product product)
    {
        return new ProductResponse(
            product.Id,
            product.Name,
            product.Stock,
            product.MinimumStock,
            product.IsOutOfStock,
            product.IsLowStock,
            product.CreatedAtUtc,
            product.UpdatedAtUtc);
    }
}
