using AutoMapper;
using GumusFit.Application.DTOs;
using GumusFit.Domain.Entities;

namespace GumusFit.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<CalorieEntry, CalorieEntryDto>();
    }
}
