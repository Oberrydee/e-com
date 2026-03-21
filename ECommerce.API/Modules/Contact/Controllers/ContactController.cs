using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ECommerce.API.Modules.Contact.DTOs;
using ECommerce.API.Modules.Contact.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Modules.Contact.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpGet("admin-contact")]
    public async Task<IActionResult> GetAdminContact()
    {
        var adminContact = await _contactService.GetAdminContactAsync();
        return Ok(adminContact);
    }

    [HttpPut("admin-contact")]
    [Authorize(Policy = "ProductManagement")]
    public async Task<IActionResult> UpdateAdminContact([FromBody] UpdateAdminContactRequestDto request)
    {
        var adminUserIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        if (!int.TryParse(adminUserIdClaim, out var adminUserId))
        {
            return Unauthorized();
        }

        var adminContact = await _contactService.UpdateAdminContactAsync(request, adminUserId);
        return Ok(adminContact);
    }

    [HttpPost("user-contact-requests")]
    public async Task<IActionResult> CreateUserContactRequest([FromBody] CreateUserContactRequestDto request)
    {
        var contactRequest = await _contactService.CreateUserContactRequestAsync(request);
        return Created(string.Empty, contactRequest);
    }
}
