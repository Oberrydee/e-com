using ECommerce.API.Common.Enums;

namespace ECommerce.API.Modules.Contact.Entities;

public class UserContactRequest
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public int UserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public ContactRequestStatus Status { get; set; } = ContactRequestStatus.New;
}
