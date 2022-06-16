using AutoMapper;
using EmployeeRestApiLibrary.Dtos;
using EmployeeRestApiLibrary.Models;

namespace EmployeeRestApi.Mappings;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        CreateMap<AddressDto, Address>()
            .ForMember(
                dest => dest.City,
                opt => opt.MapFrom(src => $"{src.City}")
            )
            .ForMember(
                dest => dest.Street,
                opt => opt.MapFrom(src => $"{src.Street}")
            )
            .ForMember(dest => dest.PostCode,
                opt => opt.MapFrom(src => $"{src.PostCode}")
            )
            /*.ForMember(dest => dest.Employee, 
                src => src.Ignore()
            )*/
            .ForMember(dest => dest.AddressId, 
                src => src.Ignore()
            )
            .ForMember(dest => dest.EmployeeId, 
            src => src.Ignore()
            );

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
                dest => dest.Manager, opt => opt.MapFrom(src => $"{src.Manager}")
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
            /*.ForMember(dest => dest.Id, 
                src => src.Ignore()
            )*/;
    }

}