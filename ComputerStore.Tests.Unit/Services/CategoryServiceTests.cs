using AutoMapper;
using ComputerStore.Application.DTOs.Category;
using ComputerStore.Application.Exceptions;
using ComputerStore.Application.Mappings;
using ComputerStore.Application.Services;
using ComputerStore.Domain.Entities;
using ComputerStore.Domain.Interfaces;
using Moq;
using Xunit;

namespace ComputerStore.Tests.Unit.Services;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _mockRepo;
    private readonly IMapper _mapper;
    private readonly CategoryService _service;

    public CategoryServiceTests()
    {
        _mockRepo = new Mock<ICategoryRepository>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
        _service = new CategoryService(_mockRepo.Object, _mapper);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsCategory()
    {
        // Arrange
        var category = new Category { Id = 1, Name = "Electronics" };
        _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(category);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Electronics", result.Name);
    }

    [Fact]
    public async Task GetByIdAsync_NonExistingId_ThrowsNotFoundException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((Category?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _service.GetByIdAsync(99));
    }

    [Fact]
    public async Task CreateAsync_ValidCategory_ReturnsCreatedCategory()
    {
        // Arrange
        var dto = new CreateCategoryDto { Name = "New Category" };
        var category = new Category { Id = 1, Name = "New Category" };

        _mockRepo.Setup(r => r.GetByNameAsync("New Category")).ReturnsAsync((Category?)null);
        _mockRepo.Setup(r => r.CreateAsync(It.IsAny<Category>())).ReturnsAsync(category);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Category", result.Name);
    }

    [Fact]
    public async Task CreateAsync_DuplicateName_ThrowsValidationException()
    {
        // Arrange
        var dto = new CreateCategoryDto { Name = "Existing Category" };
        var existing = new Category { Id = 1, Name = "Existing Category" };

        _mockRepo.Setup(r => r.GetByNameAsync("Existing Category")).ReturnsAsync(existing);

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() => _service.CreateAsync(dto));
    }
}