using AutoMapper;
using ComputerStore.Application.DTOs.Product;
using ComputerStore.Application.Exceptions;
using ComputerStore.Application.Interfaces;
using ComputerStore.Domain.Entities;
using ComputerStore.Domain.Interfaces;

namespace ComputerStore.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository,
        ICategoryRepository categoryRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductResponseDto>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
    }

    public async Task<ProductResponseDto> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Product", id);

        return _mapper.Map<ProductResponseDto>(product);
    }

    public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto)
    {
        await ValidateCategoriesExistAsync(dto.CategoryIds);

        var product = _mapper.Map<Product>(dto);
        product.ProductCategories = dto.CategoryIds
            .Select(cid => new ProductCategory { CategoryId = cid })
            .ToList();

        var created = await _productRepository.CreateAsync(product);
        var full = await _productRepository.GetByIdAsync(created.Id);
        return _mapper.Map<ProductResponseDto>(full!);
    }

    public async Task<ProductResponseDto> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _productRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Product", id);

        await ValidateCategoriesExistAsync(dto.CategoryIds);

        _mapper.Map(dto, product);
        product.ProductCategories = dto.CategoryIds
            .Select(cid => new ProductCategory { ProductId = id, CategoryId = cid })
            .ToList();

        var updated = await _productRepository.UpdateAsync(product);
        var full = await _productRepository.GetByIdAsync(updated.Id);
        return _mapper.Map<ProductResponseDto>(full!);
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Product", id);

        await _productRepository.DeleteAsync(product);
    }

    private async Task ValidateCategoriesExistAsync(List<int> categoryIds)
    {
        var errors = new List<string>();
        foreach (var cid in categoryIds)
        {
            if (!await _categoryRepository.ExistsAsync(cid))
                errors.Add($"Category with id '{cid}' does not exist.");
        }
        if (errors.Any())
            throw new ValidationException(errors);
    }
}