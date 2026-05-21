using System.ComponentModel.DataAnnotations;

namespace ComputerStore.Application.DTOs.Stock;

public class StockImportItemDto
{
    [Required(ErrorMessage = "Product name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "At least one category must be provided.")]
    public List<string> Categories { get; set; } = new();

    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
    public int Quantity { get; set; }
}

public class StockImportResultDto
{
    public int TotalProcessed { get; set; }
    public int ProductsCreated { get; set; }
    public int ProductsUpdated { get; set; }
    public int CategoriesCreated { get; set; }
    public List<string> Errors { get; set; } = new();
}