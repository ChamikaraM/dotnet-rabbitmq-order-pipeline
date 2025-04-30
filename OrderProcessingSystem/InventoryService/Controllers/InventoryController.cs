using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
    private readonly InventoryDbContext _db;

    public InventoryController(InventoryDbContext db)
    {
        _db = db;
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetStock(Guid productId)
    {
        var stock = await _db.Stocks.FindAsync(productId);
        if (stock == null) return NotFound();
        return Ok(stock);
    }

    [HttpPost]
    public async Task<IActionResult> AddStock([FromBody] Stock stock)
    {
        _db.Stocks.Add(stock);
        await _db.SaveChangesAsync();
        return Ok(stock);
    }
}
