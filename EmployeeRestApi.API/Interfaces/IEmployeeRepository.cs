using EmployeeRestApiLibrary.Enumerations;
using EmployeeRestApiLibrary.Models;

namespace EmployeeRestApi.Interfaces;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAll();
    Task<Employee> GetById(long id);
    Task<Employee> GetByNameAndBirthdateInterval(string LastName, DateTime birthDateRangeMin
        , DateTime birthDateRangeMax);
    Task<List<Employee>> GetAllByManagerId(long id);
    Task<EmployeeStatisticsByRole> GetStatisticsByJobRole(JobRole jobRole);
    Task Create(Employee employee);
    Task Update(long id);
    Task UpdateSalary(long id, decimal salary);
    Task Delete(long id);
}