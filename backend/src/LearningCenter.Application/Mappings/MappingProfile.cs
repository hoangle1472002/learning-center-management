using AutoMapper;
using LearningCenter.Application.DTOs.Auth;
using LearningCenter.Domain.Entities;

namespace LearningCenter.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore()); // Will be populated manually

        // Add more mappings as needed
    }
}
