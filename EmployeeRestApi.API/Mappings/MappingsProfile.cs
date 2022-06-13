using AutoMapper;
using EmployeeRestApiLibrary.Dtos;
using EmployeeRestApiLibrary.Models;

namespace EmployeeRestApi.Mappings;

public class MappingsProfile : Profile
{
    public MappingsProfile()
    {
        CreateMap<EmployeeDto, Employee>().ReverseMap();
        CreateMap<AddressDto, Address>().ReverseMap();
    }
}