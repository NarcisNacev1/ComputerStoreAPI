using ComputerStore.Application.DTOs.Basket;
using ComputerStore.Application.Exceptions;
using ComputerStore.Application.Interfaces;
using ComputerStore.Domain.Entities;
using ComputerStore.Domain.Interfaces;

namespace ComputerStore.Application.Services;

public class BasketService : IBasketService
{
    private readonly IProductRepository _productRepository;

    public BasketService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<DiscountResultDto> CalculateDiscountAsync(BasketDto basket)
    {
        var productIds = basket.Items.Select(i => i.ProductId).Distinct().ToList();
        var products = await _productRepository.GetByIdsAsync(productIds);
        var productDict = products.ToDictionary(p => p.Id);

        foreach (var item in basket.Items)
        {
            if (!productDict.ContainsKey(item.ProductId))
                throw new NotFoundException($"Product with id '{item.ProductId}' was not found.");

            var product = productDict[item.ProductId];
            if (product.Quantity < item.Quantity)
                throw new InsufficientStockException(product.Name, item.Quantity, product.Quantity);
        }

        var result = new DiscountResultDto();
        decimal subTotal = 0;

        foreach (var item in basket.Items)
        {
            var product = productDict[item.ProductId];
            var discountPercent = CalculateDiscountPercent(product, basket.Items);
            var discountedUnitPrice = product.Price * (1 - discountPercent / 100);
            var lineTotal = discountedUnitPrice * item.Quantity;
            var discountAmount = (product.Price - discountedUnitPrice) * item.Quantity;

            result.Lines.Add(new DiscountLineDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Quantity = item.Quantity,
                UnitPrice = product.Price,
                DiscountPercent = discountPercent,
                DiscountedUnitPrice = discountedUnitPrice,
                LineTotal = lineTotal
            });

            subTotal += product.Price * item.Quantity;
            result.TotalDiscount += discountAmount;
        }

        result.SubTotal = subTotal;
        result.FinalTotal = subTotal - result.TotalDiscount;

        return result;
    }

    private decimal CalculateDiscountPercent(Product product, List<BasketItemDto> items)
    {
        var categories = product.ProductCategories.Select(pc => pc.CategoryId).ToList();
        var maxDiscount = 0m;

        foreach (var categoryId in categories)
        {
            var itemsInSameCategory = items
                .Where(item => item.ProductId != product.Id)
                .Where(item =>
                {
                    var p = _productRepository.GetByIdAsync(item.ProductId).Result;
                    return p != null && p.ProductCategories.Any(pc => pc.CategoryId == categoryId);
                })
                .Sum(item => item.Quantity);

            if (itemsInSameCategory > 0)
                maxDiscount = Math.Max(maxDiscount, 5m);
        }

        return maxDiscount;
    }
}