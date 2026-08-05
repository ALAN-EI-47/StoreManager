using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StoreManager.Api.Data;
using StoreManager.Api.Models;

namespace StoreManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAll()
    {
        var products = await db.Products
            .Include(p => p.Category)
            .OrderBy(p => p.Name)
            .ToListAsync();

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var product = await db.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<Product>> Create(Product product)
    {
        product.Id = 0;
        product.CreatedAt = DateTime.Now;

        db.Products.Add(product);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest("Route id and body id do not match.");
        }

        var existing = await db.Products.FindAsync(id);
        if (existing is null)
        {
            return NotFound();
        }

        existing.CategoryId = product.CategoryId;
        existing.Name = product.Name;
        existing.Barcode = product.Barcode;
        existing.Unit = product.Unit;
        existing.CostPrice = product.CostPrice;
        existing.SellingPrice = product.SellingPrice;
        existing.StockQuantity = product.StockQuantity;

        await db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        db.Products.Remove(product);
        await db.SaveChangesAsync();

        return NoContent();
    }
}