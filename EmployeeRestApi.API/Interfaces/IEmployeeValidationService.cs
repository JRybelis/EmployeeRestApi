using EmployeeRestApiLibrary.Dtos;
using EmployeeRestApiLibrary.Enumerations;

namespace EmployeeRestApi.Interfaces;

public interface IEmployeeValidationService
{
    Task<bool> IsDtoValidationSuccess(EmployeeDto employeeDto, IEmployeeRepository repository);
    Task<bool> IsDtoValidationSuccess(EmployeeDto employeeDto, IEmployeeRepository repository, long id);
    Task<bool> IsJobRoleTheCeo(JobRole jobRole);
    Task<bool> IsFirstNameAndLastNameTheSame(string firstName, string lastName);
    Task<bool> IsEmployeeBetweenTheAgesOf18And70(DateTime birthDate);
    Task<bool> IsEmploymentCommencementDateLaterThan20000101(DateTime startDate);
    Task<bool> IsEmploymentCommencementDateLaterThanPresent(DateTime startDate);
    Task<bool> IsCurrentSalaryAPositiveAmount(decimal salary);
}