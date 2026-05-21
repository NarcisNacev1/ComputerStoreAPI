using ComputerStore.Application.DTOs.Stock;
using ComputerStore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ComputerStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockController : ControllerBase
{
    private readonly IStockImportService _stockImportService;

    public StockController(IStockImportService stockImportService)
    {
        _stockImportService = stockImportService;
    }

    [HttpPost("import")]
    [ProducesResponseType(typeof(StockImportResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Import([FromBody] List<StockImportItemDto> items)
    {
        if (items == null || !items.Any())
            return BadRequest(new { errors = new[] { "Import list cannot be empty." } });

        var result = await _stockImportService.ImportAsync(items);
        return Ok(result);
    }
}