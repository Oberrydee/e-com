namespace ECommerce.API.Modules.Contact.DTOs;

public class UserContactRequestResponseDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = string.Empty;
}
