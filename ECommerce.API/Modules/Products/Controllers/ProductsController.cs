using ECommerce.API.Modules.Products.DTOs;
using ECommerce.API.Modules.Products.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Modules.Products.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _productService.GetAllAsync();
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [Authorize(Policy = "ProductManagement")]
    public async Task<IActionResult> Create([FromForm] ProductRequestDto request)
    {
        var product = await _productService.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    [Consumes("multipart/form-data")]
    [Authorize(Policy = "ProductManagement")]
    public async Task<IActionResult> Update(int id, [FromForm] ProductRequestDto request)
    {
        var product = await _productService.UpdateAsync(id, request);
        return product is null ? NotFound() : Ok(product);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Policy = "ProductManagement")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productService.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
}
