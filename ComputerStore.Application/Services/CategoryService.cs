using AutoMapper;
using ComputerStore.Application.DTOs.Category;
using ComputerStore.Application.Exceptions;
using ComputerStore.Application.Interfaces;
using ComputerStore.Domain.Entities;
using ComputerStore.Domain.Interfaces;

namespace ComputerStore.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryResponseDto>> GetAllAsync()
    {
        var categories = await _categoryRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
    }

    public async Task<CategoryResponseDto> GetByIdAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Category", id);

        return _mapper.Map<CategoryResponseDto>(category);
    }

    public async Task<CategoryResponseDto> CreateAsync(CreateCategoryDto dto)
    {
        var existing = await _categoryRepository.GetByNameAsync(dto.Name);
        if (existing != null)
            throw new ValidationException($"A category with the name '{dto.Name}' already exists.");

        var category = _mapper.Map<Category>(dto);
        var created = await _categoryRepository.CreateAsync(category);
        return _mapper.Map<CategoryResponseDto>(created);
    }

    public async Task<CategoryResponseDto> UpdateAsync(int id, UpdateCategoryDto dto)
    {
        var category = await _categoryRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Category", id);

        var duplicate = await _categoryRepository.GetByNameAsync(dto.Name);
        if (duplicate != null && duplicate.Id != id)
            throw new ValidationException($"A category with the name '{dto.Name}' already exists.");

        _mapper.Map(dto, category);
        var updated = await _categoryRepository.UpdateAsync(category);
        return _mapper.Map<CategoryResponseDto>(updated);
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id)
            ?? throw new NotFoundException("Category", id);

        await _categoryRepository.DeleteAsync(category);
    }
}