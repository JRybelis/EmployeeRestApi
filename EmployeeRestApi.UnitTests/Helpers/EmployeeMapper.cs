using EmployeeRestApiLibrary.Dtos;
using EmployeeRestApiLibrary.Models;

namespace EmployeeRestApiUnitTests.Helpers;

internal static class EmployeeMapper
{
    public static Employee AsEntity(this EmployeeDto employeeDto)
    {
        return new Employee
        {
            FirstName = employeeDto.FirstName
            , LastName = employeeDto.LastName
            , BirthDate = employeeDto.BirthDate
            , EmploymentCommencementDate = employeeDto.EmploymentCommencementDate
            , ManagerId = employeeDto.ManagerId
            , HomeAddress = employeeDto.HomeAddress
            , CurrentSalary = employeeDto.CurrentSalary
            , Role = employeeDto.Role
        };
    }

    public static EmployeeDto AsDto(this Employee employee)
    {
        return new EmployeeDto
        {
            FirstName = employee.FirstName
            , LastName = employee.LastName
            , BirthDate = employee.BirthDate
            , EmploymentCommencementDate = employee.EmploymentCommencementDate
            , ManagerId = employee.ManagerId
            , HomeAddress = employee.HomeAddress
            , CurrentSalary = employee.CurrentSalary
            , Role = employee.Role
        };
    }
}