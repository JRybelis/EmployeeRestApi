using EmployeeRestApiLibrary.Dtos;
using EmployeeRestApiLibrary.Models;

namespace EmployeeRestApi.Interfaces;

public interface IEmployeeRepository
{
    Task<List<EmployeeDto>> GetAll();
    Task<EmployeeDto> GetById(long id);
    Task<EmployeeDto> GetByNameAndBirthdateInterval(string FirstName, string LastName
        , DateTime birthDateRangeMin, DateTime birthDateRangeMax);
    Task<List<EmployeeDto>> GetAllByManagerId(long id);
    Task Create(Employee employee);
    Task Update(long id);
    Task UpdateSalary(long id, decimal salary);
    Task Delete(long id);
}