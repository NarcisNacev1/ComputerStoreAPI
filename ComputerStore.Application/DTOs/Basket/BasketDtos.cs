using System.ComponentModel.DataAnnotations;

namespace ComputerStore.Application.DTOs.Basket;

public class BasketItemDto
{
    [Required]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
    public int Quantity { get; set; }
}

public class BasketDto
{
    [Required(ErrorMessage = "Basket must contain at least one item.")]
    [MinLength(1)]
    public List<BasketItemDto> Items { get; set; } = new();
}

public class DiscountLineDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal DiscountPercent { get; set; }
    public decimal DiscountedUnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}

public class DiscountResultDto
{
    public List<DiscountLineDto> Lines { get; set; } = new();
    public decimal SubTotal { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal FinalTotal { get; set; }
}