using AutoMapper;
using EmployeeRestApiLibrary.Dtos;
using EmployeeRestApiLibrary.Models;

namespace EmployeeRestApi.Mappings;

public class MappingsProfile : Profile
{
    public MappingsProfile()
    {
        CreateMap<EmployeeDto, Employee>().ReverseMap().ForMember(dest => dest.HomeAddress
            , act => act.MapFrom(src => src.HomeAddress));
        CreateMap<AddressDto, Address>().ReverseMap();
    }
}