using ComputerStore.Application.DTOs.Category;
using ComputerStore.Application.DTOs.Product;
using ComputerStore.Application.DTOs.Stock;
using ComputerStore.Application.DTOs.Basket;

namespace ComputerStore.Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAllAsync();
    Task<CategoryResponseDto> GetByIdAsync(int id);
    Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto);
    Task<CategoryResponseDto> UpdateAsync(int id, UpdateCategoryDto dto);
    Task DeleteAsync(int id);
}

public interface IProductService
{
    Task<IEnumerable<ProductResponseDto>> GetAllAsync();
    Task<ProductResponseDto> GetByIdAsync(int id);
    Task<ProductResponseDto> CreateAsync(CreateProductDto dto);
    Task<ProductResponseDto> UpdateAsync(int id, UpdateProductDto dto);
    Task DeleteAsync(int id);
}

public interface IStockImportService
{
    Task<StockImportResultDto> ImportAsync(IEnumerable<StockImportItemDto> items);
}

public interface IBasketService
{
    Task<DiscountResultDto> CalculateDiscountAsync(BasketDto basket);
}