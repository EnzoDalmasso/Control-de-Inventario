using Inventory.Domain.Entities;
using Inventory.Domain.Exceptions;

namespace Inventory.Tests;

public sealed class ProductTests
{
    [Fact]
    public void Create_WithValidData_StoresInitialStock()
    {
        var product = new Product("Teclado", 10);

        Assert.Equal("Teclado", product.Name);
        Assert.Equal(10, product.Stock);
        Assert.Equal(5, product.MinimumStock);
    }

    [Fact]
    public void Create_WithNegativeStock_ThrowsDomainException()
    {
        Assert.Throws<DomainException>(() => new Product("Mouse", -1));
    }

    [Fact]
    public void DecreaseStock_WithInsufficientStock_ThrowsDomainException()
    {
        var product = new Product("Monitor", 2);

        Assert.Throws<DomainException>(() => product.DecreaseStock(3));
    }

    [Fact]
    public void IncreaseAndDecreaseStock_UpdatesStock()
    {
        var product = new Product("Auriculares", 5);

        product.IncreaseStock(4);
        product.DecreaseStock(3);

        Assert.Equal(6, product.Stock);
    }
}
