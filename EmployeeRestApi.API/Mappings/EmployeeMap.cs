using EmployeeRestApiLibrary.Dtos;
using EmployeeRestApiLibrary.Models;

namespace EmployeeRestApi.Mappings;

public static class EmployeeMap
{
    public static Employee AsEntity(this EmployeeDto employeeDto)
    {
        return new Employee
        {
            FirstName = employeeDto.FirstName
            , LastName = employeeDto.LastName
            , BirthDate = employeeDto.BirthDate
            , StartDate = employeeDto.StartDate
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
            , StartDate = employee.StartDate
            , ManagerId = employee.ManagerId
            , HomeAddress = employee.HomeAddress
            , CurrentSalary = employee.CurrentSalary
            , Role = employee.Role
        };
    }
}