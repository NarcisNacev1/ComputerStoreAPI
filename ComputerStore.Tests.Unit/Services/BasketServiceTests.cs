using ComputerStore.Application.DTOs.Basket;
using ComputerStore.Application.Exceptions;
using ComputerStore.Application.Services;
using ComputerStore.Domain.Entities;
using ComputerStore.Domain.Interfaces;
using Moq;
using Xunit;

namespace ComputerStore.Tests.Unit.Services;

public class BasketServiceTests
{
    private readonly Mock<IProductRepository> _mockRepo;
    private readonly BasketService _service;

    public BasketServiceTests()
    {
        _mockRepo = new Mock<IProductRepository>();
        _service = new BasketService(_mockRepo.Object);
    }

    [Fact]
    public async Task CalculateDiscountAsync_SingleProduct_NoDiscount()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "CPU",
            Price = 100m,
            Quantity = 10,
            ProductCategories = new List<ProductCategory>
            {
                new ProductCategory { CategoryId = 1, Category = new Category { Name = "CPU" } }
            }
        };

        var basket = new BasketDto
        {
            Items = new List<BasketItemDto> { new BasketItemDto { ProductId = 1, Quantity = 1 } }
        };

        _mockRepo.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync(new List<Product> { product });

        // Act
        var result = await _service.CalculateDiscountAsync(basket);

        // Assert
        Assert.Equal(100m, result.SubTotal);
        Assert.Equal(0m, result.TotalDiscount);
        Assert.Equal(100m, result.FinalTotal);
    }

    [Fact]
    public async Task CalculateDiscountAsync_TwoSameCategoryProducts_Applies5PercentDiscount()
    {
        // Arrange
        var cpu1 = new Product
        {
            Id = 1,
            Name = "Intel i9",
            Price = 500m,
            Quantity = 10,
            ProductCategories = new List<ProductCategory>
            {
                new ProductCategory { CategoryId = 1, Category = new Category { Name = "CPU" } }
            }
        };

        var cpu2 = new Product
        {
            Id = 2,
            Name = "AMD Ryzen",
            Price = 450m,
            Quantity = 10,
            ProductCategories = new List<ProductCategory>
            {
                new ProductCategory { CategoryId = 1, Category = new Category { Name = "CPU" } }
            }
        };

        var basket = new BasketDto
        {
            Items = new List<BasketItemDto>
            {
                new BasketItemDto { ProductId = 1, Quantity = 1 },
                new BasketItemDto { ProductId = 2, Quantity = 1 }
            }
        };

        _mockRepo.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync(new List<Product> { cpu1, cpu2 });

        // Act
        var result = await _service.CalculateDiscountAsync(basket);

        // Assert
        Assert.Equal(950m, result.SubTotal);
        Assert.Equal(22.5m, result.TotalDiscount); // 5% off one product (500 * 0.05 = 25) or (450 * 0.05 = 22.5)
        Assert.Equal(927.5m, result.FinalTotal);
    }

    [Fact]
    public async Task CalculateDiscountAsync_InsufficientStock_ThrowsException()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Keyboard", Price = 50m, Quantity = 1 };
        var basket = new BasketDto
        {
            Items = new List<BasketItemDto> { new BasketItemDto { ProductId = 1, Quantity = 5 } }
        };

        _mockRepo.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
            .ReturnsAsync(new List<Product> { product });

        // Act & Assert
        await Assert.ThrowsAsync<InsufficientStockException>(
            () => _service.CalculateDiscountAsync(basket));
    }
}