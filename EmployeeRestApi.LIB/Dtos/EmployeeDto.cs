using EmployeeRestApiLibrary.Enumerations;
using EmployeeRestApiLibrary.Models;

namespace EmployeeRestApiLibrary.Dtos;

public class EmployeeDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime EmploymentCommencementDate { get; set; }
    public Employee Manager { get; set; }
    public AddressDto HomeAddress { get; set; }
    public decimal CurrentSalary { get; set; }
    public JobRole Role { get; set; }
}