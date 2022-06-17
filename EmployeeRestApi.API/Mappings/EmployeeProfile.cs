using AutoMapper;
using EmployeeRestApiLibrary.Dtos;
using EmployeeRestApiLibrary.Models;

namespace EmployeeRestApi.Mappings;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<EmployeeDto, Employee>()
            .ForMember(
                dest => dest.FirstName,
                opt => opt.MapFrom(src => $"{src.FirstName}")
            )
            .ForMember(
                dest => dest.LastName,
                opt => opt.MapFrom(src => $"{src.LastName}")
            )
            .ForMember(
                dest => dest.BirthDate,
                opt => opt.MapFrom(src => $"{src.BirthDate}")
            )
            .ForMember(
                dest => dest.EmploymentCommencementDate, opt => opt.MapFrom(src => $"{src.EmploymentCommencementDate}")
            )
            .ForMember(
                dest => dest.ManagerId, opt => opt.Condition((src, dest, srcMember) => srcMember != null)    
            )
            .ForMember(
                dest => dest.ManagerId, opt => opt.MapFrom(src => $"{src.ManagerId}")
            )
            .ForMember(
                dest => dest.HomeAddress, opt => opt.MapFrom(src => $"{src.HomeAddress}")
            )
            .ForMember(
                dest => dest.CurrentSalary, opt => opt.MapFrom(src => $"{src.CurrentSalary}")
            )
            .ForMember(
                dest => dest.Role, opt => opt.MapFrom(src => $"{src.Role}")
            )
            ;
    }

}