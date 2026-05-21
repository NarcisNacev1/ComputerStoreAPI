using System.ComponentModel.DataAnnotations;

namespace ComputerStore.Application.DTOs.Product;

public class CreateProductDto
{
    [Required(ErrorMessage = "Product name is required.")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "At least one category must be provided.")]
    [MinLength(1, ErrorMessage = "At least one category must be provided.")]
    public List<int> CategoryIds { get; set; } = new();
}

public class UpdateProductDto
{
    [Required(ErrorMessage = "Product name is required.")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "At least one category must be provided.")]
    [MinLength(1)]
    public List<int> CategoryIds { get; set; } = new();
}

public class ProductResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public List<string> Categories { get; set; } = new();
}