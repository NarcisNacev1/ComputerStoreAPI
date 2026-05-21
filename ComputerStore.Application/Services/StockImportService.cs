using ComputerStore.Application.DTOs.Stock;
using ComputerStore.Application.Interfaces;
using ComputerStore.Domain.Entities;
using ComputerStore.Domain.Interfaces;

namespace ComputerStore.Application.Services;

public class StockImportService : IStockImportService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public StockImportService(IProductRepository productRepository,
        ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<StockImportResultDto> ImportAsync(IEnumerable<StockImportItemDto> items)
    {
        var result = new StockImportResultDto();

        foreach (var item in items)
        {
            try
            {
                result.TotalProcessed++;

                var categoryIds = new List<int>();
                foreach (var categoryName in item.Categories)
                {
                    var trimmed = categoryName.Trim();
                    var category = await _categoryRepository.GetByNameAsync(trimmed);
                    if (category == null)
                    {
                        category = await _categoryRepository.CreateAsync(new Category { Name = trimmed });
                        result.CategoriesCreated++;
                    }
                    categoryIds.Add(category.Id);
                }

                var product = await _productRepository.GetByNameAsync(item.Name);
                if (product == null)
                {
                    product = new Product
                    {
                        Name = item.Name,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        ProductCategories = categoryIds.Select(cid => new ProductCategory { CategoryId = cid }).ToList()
                    };
                    await _productRepository.CreateAsync(product);
                    result.ProductsCreated++;
                }
                else
                {
                    product.Price = item.Price;
                    product.Quantity += item.Quantity;
                    product.ProductCategories = categoryIds
                        .Select(cid => new ProductCategory { ProductId = product.Id, CategoryId = cid })
                        .ToList();
                    await _productRepository.UpdateAsync(product);
                    result.ProductsUpdated++;
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Failed to import '{item.Name}': {ex.Message}");
            }
        }

        return result;
    }
}