using AutoMapper;
using EmployeeRestApiLibrary.Dtos;
using EmployeeRestApiLibrary.Models;

namespace EmployeeRestApi.Mappings;

public class MappingsProfile : Profile
{
    public Employee MapAsEntity(EmployeeDto employeeDto)
    {
        var config = new MapperConfiguration(config =>
        {
            config.CreateMap<EmployeeDto, Employee>();
            config.CreateMap<AddressDto, Address>();
        });
        config.AssertConfigurationIsValid();

        var mapper = config.CreateMapper();
        
        return mapper.Map<EmployeeDto, Employee>(employeeDto);
    }
    
    public EmployeeDto MapAsDto(Employee employee)
    {
        var config = new MapperConfiguration(config =>
        {
            config.CreateMap<Employee, EmployeeDto>();
            config.CreateMap<Address, AddressDto>();
        });
        config.AssertConfigurationIsValid();

        var mapper = config.CreateMapper();
        
        return mapper.Map<Employee, EmployeeDto>(employee);
    }
    /*public MappingsProfile()
    {
        CreateMap<EmployeeDto, Employee>().ReverseMap().ForMember(dest => dest.HomeAddress
            , act => act.MapFrom(src => src.HomeAddress));
        CreateMap<AddressDto, Address>().ReverseMap();
    }*/
}