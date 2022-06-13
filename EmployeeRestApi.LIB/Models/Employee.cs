using EmployeeRestApiLibrary.Enumerations;

namespace EmployeeRestApiLibrary.Models;

public class Employee
{
    public long Id { get; set; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime BirthDate { get; set; }
    public DateTime EmploymentCommencementDate { get; set; }
    public Employee Manager { get; set; }
    public Address HomeAddress { get; set; }
    public decimal CurrentSalary { get; set; }
    public JobRole Role { get; set; }
}