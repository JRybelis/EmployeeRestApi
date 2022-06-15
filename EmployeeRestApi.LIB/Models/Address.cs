using AutoMapper;
using AutoMapper.Configuration.Annotations;
using EmployeeRestApiLibrary.Dtos;

namespace EmployeeRestApiLibrary.Models;

[AutoMap(typeof(AddressDto), ReverseMap = true)]
public class Address
{
    [SourceMember(nameof(AddressDto.Street))]
    public string Street { get; set; }
    
    
    [SourceMember(nameof(AddressDto.City))]
    public string City { get; set; }
    
    
    [SourceMember(nameof(AddressDto.Postcode))]
    public string PostCode { get; set; }
}