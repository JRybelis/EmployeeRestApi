using EmployeeRestApiLibrary.Enumerations;

namespace EmployeeRestApiLibrary.Models;

public class EmployeeStatisticsByRole
{
    public JobRole JobRole { get; set; }
    public int EmployeeCount { get; set; }
    public decimal SalaryAverage { get; set; }
}