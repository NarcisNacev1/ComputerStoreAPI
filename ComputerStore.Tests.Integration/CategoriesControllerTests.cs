using ComputerStore.Application.DTOs.Category;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ComputerStore.Tests.Integration;

public class CategoriesControllerTests : IntegrationTestBase
{
    [Fact]
    public async Task GetAllCategories_ReturnsEmptyList_WhenNoCategories()
    {
        // Act
        var response = await _client.GetAsync("/api/categories");
        var categories = await response.Content.ReadFromJsonAsync<List<CategoryResponseDto>>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Empty(categories);
    }

    [Fact]
    public async Task CreateCategory_ValidData_ReturnsCreatedCategory()
    {
        // Arrange
        var newCategory = new CreateCategoryDto { Name = "Gaming", Description = "Gaming products" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/categories", newCategory);
        var created = await response.Content.ReadFromJsonAsync<CategoryResponseDto>();

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(created);
        Assert.Equal("Gaming", created.Name);
        Assert.Equal("Gaming products", created.Description);
        Assert.True(created.Id > 0);
    }

    [Fact]
    public async Task CreateCategory_DuplicateName_ReturnsBadRequest()
    {
        // Arrange
        var category = new CreateCategoryDto { Name = "Electronics" };
        await _client.PostAsJsonAsync("/api/categories", category);

        // Act
        var response = await _client.PostAsJsonAsync("/api/categories", category);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetCategoryById_ExistingId_ReturnsCategory()
    {
        // Arrange
        var newCategory = new CreateCategoryDto { Name = "Laptops" };
        var createResponse = await _client.PostAsJsonAsync("/api/categories", newCategory);
        var created = await createResponse.Content.ReadFromJsonAsync<CategoryResponseDto>();

        // Act
        var response = await _client.GetAsync($"/api/categories/{created!.Id}");
        var category = await response.Content.ReadFromJsonAsync<CategoryResponseDto>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(created.Id, category.Id);
        Assert.Equal("Laptops", category.Name);
    }

    [Fact]
    public async Task UpdateCategory_ValidData_ReturnsUpdatedCategory()
    {
        // Arrange
        var newCategory = new CreateCategoryDto { Name = "Old Name" };
        var createResponse = await _client.PostAsJsonAsync("/api/categories", newCategory);
        var created = await createResponse.Content.ReadFromJsonAsync<CategoryResponseDto>();
        var updateDto = new UpdateCategoryDto { Name = "New Name", Description = "Updated" };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/categories/{created!.Id}", updateDto);
        var updated = await response.Content.ReadFromJsonAsync<CategoryResponseDto>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("New Name", updated.Name);
        Assert.Equal("Updated", updated.Description);
    }

    [Fact]
    public async Task DeleteCategory_ExistingId_ReturnsNoContent()
    {
        // Arrange
        var newCategory = new CreateCategoryDto { Name = "To Delete" };
        var createResponse = await _client.PostAsJsonAsync("/api/categories", newCategory);
        var created = await createResponse.Content.ReadFromJsonAsync<CategoryResponseDto>();

        // Act
        var deleteResponse = await _client.DeleteAsync($"/api/categories/{created!.Id}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // Verify it's gone
        var getResponse = await _client.GetAsync($"/api/categories/{created.Id}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}