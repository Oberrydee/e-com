using ECommerce.API.Modules.Products.DTOs;
using ECommerce.API.Modules.Products.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Modules.Products.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _cartService.GetAllAsync();
        return Ok(items);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _cartService.GetByIdAsync(id);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductInCartRequestDto request)
    {
        var item = await _cartService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductInCartRequestDto request)
    {
        var item = await _cartService.UpdateAsync(id, request);
        return item is null ? NotFound() : Ok(item);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _cartService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
