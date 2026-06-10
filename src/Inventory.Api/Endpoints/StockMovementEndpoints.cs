using Inventory.Api.Contracts.StockMovements;
using Inventory.Domain.Enums;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Endpoints;

public static class StockMovementEndpoints
{
    public static RouteGroupBuilder MapStockMovementEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/stock-movements")
            .WithTags("Stock Movements");

        group.MapGet("/", GetStockMovements)
            .WithName("GetStockMovements");

        return group;
    }

    private static async Task<IResult> GetStockMovements(
        InventoryDbContext dbContext,
        int? productId,
        StockMovementType? type,
        CancellationToken cancellationToken)
    {
        var query = dbContext.StockMovements
            .AsNoTracking()
            .Include(movement => movement.Product)
            .AsQueryable();

        if (productId.HasValue)
        {
            query = query.Where(movement => movement.ProductId == productId.Value);
        }

        if (type.HasValue)
        {
            query = query.Where(movement => movement.Type == type.Value);
        }

        var movements = await query
            .OrderByDescending(movement => movement.CreatedAtUtc)
            .Select(movement => new StockMovementResponse(
                movement.Id,
                movement.ProductId,
                movement.Product == null ? string.Empty : movement.Product.Name,
                movement.Quantity,
                movement.Type.ToString(),
                movement.CreatedAtUtc))
            .ToListAsync(cancellationToken);

        return Results.Ok(movements);
    }
}
