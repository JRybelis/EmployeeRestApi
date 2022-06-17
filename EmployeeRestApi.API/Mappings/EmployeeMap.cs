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
            , EmploymentCommencementDate = employeeDto.EmploymentCommencementDate
            , Manager = employeeDto.Manager
            , HomeAddress = 
                new Address
                {
                      Street = employeeDto.HomeAddress.Street
                    , City = employeeDto.HomeAddress.City
                    , PostCode = employeeDto.HomeAddress.PostCode
                }
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
            , Manager = employee.Manager
            , HomeAddress = new AddressDto
            {
                  
                Street = employee.HomeAddress.Street
                , City = employee.HomeAddress.City
                , PostCode = employee.HomeAddress.PostCode
            }
            , CurrentSalary = employee.CurrentSalary
            , Role = employee.Role
        };
    }
}