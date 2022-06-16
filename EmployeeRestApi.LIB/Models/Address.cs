using AutoMapper;
using AutoMapper.Configuration.Annotations;
using EmployeeRestApiLibrary.Dtos;

namespace EmployeeRestApiLibrary.Models;

[AutoMap(typeof(AddressDto), ReverseMap = true)]
public class Address
{
    [Ignore]
    public long AddressId { get; set; }
    
    [Ignore]
    public long EmployeeId { get; set; }
    
    [Ignore]
    public Employee Employee { get; set; }
    
    [SourceMember(nameof(AddressDto.Street))]
    public string Street { get; set; }
    
    
    [SourceMember(nameof(AddressDto.City))]
    public string City { get; set; }
    
    
    [SourceMember(nameof(AddressDto.PostCode))]
    public string PostCode { get; set; }
}