using ECommerce.API.Modules.Products.DTOs;
using ECommerce.API.Modules.Products.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Modules.Products.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WishlistController : ControllerBase
{
    private readonly IWishlistService _wishlistService;

    public WishlistController(IWishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var wishlists = await _wishlistService.GetAllAsync();
        return Ok(wishlists);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var wishlist = await _wishlistService.GetByIdAsync(id);
        return wishlist is null ? NotFound() : Ok(wishlist);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] WishlistRequestDto request)
    {
        var wishlist = await _wishlistService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = wishlist.Id }, wishlist);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] WishlistRequestDto request)
    {
        var wishlist = await _wishlistService.UpdateAsync(id, request);
        return wishlist is null ? NotFound() : Ok(wishlist);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _wishlistService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
