namespace Inventory.Api.Contracts.Products;

public sealed record CreateProductRequest(
    string Name,
    int Stock,
    int? MinimumStock);
