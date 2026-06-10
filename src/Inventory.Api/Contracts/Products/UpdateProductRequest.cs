namespace Inventory.Api.Contracts.Products;

public sealed record UpdateProductRequest(
    string Name,
    int MinimumStock);
