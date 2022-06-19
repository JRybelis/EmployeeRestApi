using EmployeeRestApiLibrary.Enumerations;
using EmployeeRestApiLibrary.Models;

namespace EmployeeRestApiLibrary.Dtos;

public class EmployeeDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime StartDate { get; set; }
    public long? ManagerId { get; set; }
    public string HomeAddress { get; set; }
    public decimal CurrentSalary { get; set; }
    public JobRole Role { get; set; }
}