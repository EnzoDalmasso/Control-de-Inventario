using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Data;

public sealed class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");

            entity.HasKey(product => product.Id);

            entity.Property(product => product.Name)
                .HasMaxLength(120)
                .UseCollation("NOCASE")
                .IsRequired();

            entity.HasIndex(product => product.Name)
                .IsUnique();

            entity.Property(product => product.Stock)
                .IsRequired();

            entity.Property(product => product.MinimumStock)
                .HasDefaultValue(5)
                .IsRequired();

            entity.Property(product => product.CreatedAtUtc)
                .IsRequired();

            entity.Ignore(product => product.IsOutOfStock);
            entity.Ignore(product => product.IsLowStock);
        });

        modelBuilder.Entity<StockMovement>(entity =>
        {
            entity.ToTable("StockMovements");

            entity.HasKey(movement => movement.Id);

            entity.Property(movement => movement.Quantity)
                .IsRequired();

            entity.Property(movement => movement.Type)
                .HasConversion(
                    type => type.ToString(),
                    value => Enum.Parse<StockMovementType>(value))
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(movement => movement.CreatedAtUtc)
                .IsRequired();

            entity.HasOne(movement => movement.Product)
                .WithMany()
                .HasForeignKey(movement => movement.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
