using Inventory.Api.Endpoints;
using Inventory.Infrastructure;
using Inventory.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.MapGet("/", () => Results.Ok(new
{
    Application = "Inventory API",
    Status = "Running"
}))
    .WithName("HealthCheck");

app.MapProductEndpoints();
app.MapStockMovementEndpoints();

app.Run();
