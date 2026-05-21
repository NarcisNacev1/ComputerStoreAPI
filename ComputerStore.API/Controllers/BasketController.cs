using ComputerStore.Application.DTOs.Basket;
using ComputerStore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComputerStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    [HttpPost("calculate-discount")]
    [ProducesResponseType(typeof(DiscountResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CalculateDiscount([FromBody] BasketDto basket)
    {
        var result = await _basketService.CalculateDiscountAsync(basket);
        return Ok(result);
    }
}