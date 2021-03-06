using AutoMapper;
using AutoMapper.Configuration.Annotations;
using EmployeeRestApiLibrary.Dtos;
using EmployeeRestApiLibrary.Enumerations;

namespace EmployeeRestApiLibrary.Models;

[AutoMap(typeof(EmployeeDto))]
public class Employee
{
    [Ignore] 
    public long Id { get; set; }
    
    
    [SourceMember(nameof(EmployeeDto.FirstName))]
    public string FirstName { get; set; }
    
    
    [SourceMember(nameof(EmployeeDto.LastName))]
    public string LastName { get; set; }
    
    
    [SourceMember(nameof(EmployeeDto.BirthDate))]
    public DateTime BirthDate { get; set; }
    
    
    [SourceMember(nameof(EmployeeDto.StartDate))]
    public DateTime StartDate { get; set; }
    
    
    [SourceMember(nameof(EmployeeDto.ManagerId))]
    public long? ManagerId { get; set; }

    [SourceMember(nameof(EmployeeDto.HomeAddress))]
    public string HomeAddress { get; set; }
    
    
    [SourceMember(nameof(EmployeeDto.CurrentSalary))]
    public decimal CurrentSalary { get; set; }
    
    
    [SourceMember(nameof(EmployeeDto.Role))]
    public JobRole Role { get; set; }
}