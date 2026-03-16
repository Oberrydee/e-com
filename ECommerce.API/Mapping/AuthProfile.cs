using AutoMapper;
using ECommerce.API.Modules.Auth.DTOs;
using ECommerce.API.Modules.Auth.Entities;

namespace ECommerce.API.Mapping;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<RegisterRequestDto, User>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim().ToLowerInvariant()))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.Trim()))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.Trim()))
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        CreateMap<User, AuthResponseDto>()
            .ForMember(dest => dest.Token, opt => opt.Ignore())
            .ForMember(dest => dest.ExpiresInSeconds, opt => opt.Ignore())
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));
    }
}
