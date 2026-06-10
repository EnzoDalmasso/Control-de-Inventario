using Inventory.Api.Contracts.Products;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Inventory.Domain.Exceptions;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Endpoints;

public static class ProductEndpoints
{
    public static RouteGroupBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/products")
            .WithTags("Products");

        group.MapGet("/", GetProducts)
            .WithName("GetProducts");

        group.MapGet("/{id:int}", GetProductById)
            .WithName("GetProductById");

        group.MapPost("/", CreateProduct)
            .WithName("CreateProduct");

        group.MapPut("/{id:int}", UpdateProduct)
            .WithName("UpdateProduct");

        group.MapPost("/{id:int}/stock/increase", IncreaseStock)
            .WithName("IncreaseProductStock");

        group.MapPost("/{id:int}/stock/decrease", DecreaseStock)
            .WithName("DecreaseProductStock");

        group.MapDelete("/{id:int}", DeleteProduct)
            .WithName("DeleteProduct");

        return group;
    }

    private static async Task<IResult> GetProducts(
        InventoryDbContext dbContext,
        string? search,
        bool? lowStock,
        CancellationToken cancellationToken)
    {
        var query = dbContext.Products.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(product => product.Name.Contains(search));
        }

        if (lowStock == true)
        {
            query = query.Where(product => product.Stock > 0 && product.Stock < product.MinimumStock);
        }

        var products = await query
            .OrderBy(product => product.Name)
            .Select(product => ProductResponse.From(product))
            .ToListAsync(cancellationToken);

        return Results.Ok(products);
    }

    private static async Task<IResult> GetProductById(
        int id,
        InventoryDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

        return product is null
            ? Results.NotFound()
            : Results.Ok(ProductResponse.From(product));
    }

    private static async Task<IResult> CreateProduct(
        CreateProductRequest request,
        InventoryDbContext dbContext,
        CancellationToken cancellationToken)
    {
        if (await ProductNameExists(dbContext, request.Name, null, cancellationToken))
        {
            return Results.Conflict(new { message = "Ya existe un producto con ese nombre." });
        }

        try
        {
            var product = new Product(request.Name, request.Stock, request.MinimumStock ?? 5);

            dbContext.Products.Add(product);
            await dbContext.SaveChangesAsync(cancellationToken);

            var response = ProductResponse.From(product);

            return Results.Created($"/api/products/{product.Id}", response);
        }
        catch (DomainException exception)
        {
            return Results.BadRequest(new { message = exception.Message });
        }
    }

    private static async Task<IResult> UpdateProduct(
        int id,
        UpdateProductRequest request,
        InventoryDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

        if (product is null)
        {
            return Results.NotFound();
        }

        if (await ProductNameExists(dbContext, request.Name, id, cancellationToken))
        {
            return Results.Conflict(new { message = "Ya existe otro producto con ese nombre." });
        }

        try
        {
            product.Rename(request.Name);
            product.UpdateMinimumStock(request.MinimumStock);

            await dbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok(ProductResponse.From(product));
        }
        catch (DomainException exception)
        {
            return Results.BadRequest(new { message = exception.Message });
        }
    }

    private static async Task<IResult> IncreaseStock(
        int id,
        StockOperationRequest request,
        InventoryDbContext dbContext,
        CancellationToken cancellationToken)
    {
        return await ApplyStockOperation(
            id,
            request,
            StockMovementType.Entry,
            dbContext,
            cancellationToken);
    }

    private static async Task<IResult> DecreaseStock(
        int id,
        StockOperationRequest request,
        InventoryDbContext dbContext,
        CancellationToken cancellationToken)
    {
        return await ApplyStockOperation(
            id,
            request,
            StockMovementType.Exit,
            dbContext,
            cancellationToken);
    }

    private static async Task<IResult> DeleteProduct(
        int id,
        InventoryDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

        if (product is null)
        {
            return Results.NotFound();
        }

        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Results.NoContent();
    }

    private static async Task<IResult> ApplyStockOperation(
        int id,
        StockOperationRequest request,
        StockMovementType movementType,
        InventoryDbContext dbContext,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .FirstOrDefaultAsync(product => product.Id == id, cancellationToken);

        if (product is null)
        {
            return Results.NotFound();
        }

        try
        {
            if (movementType == StockMovementType.Entry)
            {
                product.IncreaseStock(request.Quantity);
            }
            else
            {
                product.DecreaseStock(request.Quantity);
            }

            var movement = new StockMovement(product.Id, request.Quantity, movementType);

            dbContext.StockMovements.Add(movement);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Results.Ok(ProductResponse.From(product));
        }
        catch (DomainException exception)
        {
            return Results.BadRequest(new { message = exception.Message });
        }
    }

    private static async Task<bool> ProductNameExists(
        InventoryDbContext dbContext,
        string name,
        int? currentProductId,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return false;
        }

        var normalizedName = name.Trim();

        return await dbContext.Products.AnyAsync(
            product => product.Name == normalizedName && product.Id != currentProductId,
            cancellationToken);
    }
}
