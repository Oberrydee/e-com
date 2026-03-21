using ECommerce.API.Modules.Contact.DTOs;

namespace ECommerce.API.Modules.Contact.Services;

public interface IContactService
{
    Task<UserContactRequestResponseDto> GetAdminContactAsync();
    Task<UserContactRequestResponseDto> UpdateAdminContactAsync(UpdateAdminContactRequestDto request, int adminUserId);
    Task<UserContactRequestResponseDto> CreateUserContactRequestAsync(CreateUserContactRequestDto request);
}
