using AutoMapper;
using ECommerce.API.Modules.Contact.DTOs;
using ECommerce.API.Modules.Contact.Entities;

namespace ECommerce.API.Mapping;

public class ContactProfile : Profile
{
    public ContactProfile()
    {
        CreateMap<CreateUserContactRequestDto, UserContactRequest>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim()))
            .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Message.Trim()))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Status, opt => opt.Ignore());

        CreateMap<UserContactRequest, UserContactRequestResponseDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
