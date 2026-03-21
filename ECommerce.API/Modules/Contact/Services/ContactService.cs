using ECommerce.API.Common.Enums;
using ECommerce.API.Data;
using ECommerce.API.Modules.Contact.DTOs;
using ECommerce.API.Modules.Contact.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Modules.Contact.Services;

public class ContactService : IContactService
{
    private static readonly DateTime EmptyCreatedAt = new(1, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    private readonly ApplicationDbContext _dbContext;

    public ContactService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserContactRequestResponseDto> GetAdminContactAsync()
    {
        var adminContact = await GetOrCreateAdminContactAsync();
        return MapToResponse(adminContact);
    }

    public async Task<UserContactRequestResponseDto> UpdateAdminContactAsync(UpdateAdminContactRequestDto request, int adminUserId)
    {
        var adminContact = await GetOrCreateAdminContactAsync();
        adminContact.Email = request.Email.Trim();
        adminContact.UserId = adminUserId;

        await _dbContext.SaveChangesAsync();

        return MapToResponse(adminContact);
    }

    public async Task<UserContactRequestResponseDto> CreateUserContactRequestAsync(CreateUserContactRequestDto request)
    {
        var contactRequest = new UserContactRequest
        {
            Id = Guid.NewGuid(),
            Email = request.Email.Trim(),
            UserId = request.UserId,
            Message = request.Message.Trim(),
            CreatedAt = DateTime.UtcNow,
            Status = ContactRequestStatus.New
        };

        _dbContext.UserContactRequests.Add(contactRequest);
        await _dbContext.SaveChangesAsync();

        return MapToResponse(contactRequest);
    }

    private async Task<UserContactRequest> GetOrCreateAdminContactAsync()
    {
        var adminContact = await _dbContext.UserContactRequests.FirstOrDefaultAsync(item => item.Id == Guid.Empty);
        if (adminContact is not null)
        {
            return adminContact;
        }

        adminContact = new UserContactRequest
        {
            Id = Guid.Empty,
            Email = string.Empty,
            UserId = 0,
            Message = string.Empty,
            CreatedAt = EmptyCreatedAt,
            Status = ContactRequestStatus.New
        };

        _dbContext.UserContactRequests.Add(adminContact);
        await _dbContext.SaveChangesAsync();

        return adminContact;
    }

    private static UserContactRequestResponseDto MapToResponse(UserContactRequest contactRequest) => new()
    {
        Id = contactRequest.Id,
        Email = contactRequest.Email,
        UserId = contactRequest.UserId,
        Message = contactRequest.Message,
        CreatedAt = contactRequest.CreatedAt,
        Status = contactRequest.Status.ToString()
    };
}
