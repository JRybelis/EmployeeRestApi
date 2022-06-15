using EmployeeRestApiLibrary.Enumerations;
using EmployeeRestApiLibrary.Models;

namespace EmployeeRestApi.Interfaces;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetAll();
    Task<IEnumerable<Employee>> GetAllByManagerId(long id);
    Task<Employee> GetById(long id);
    Task<Employee> GetByNameAndBirthdateInterval(string LastName, DateTime birthDateRangeMin
        , DateTime birthDateRangeMax);
    Task<EmployeeStatisticsByRole> GetStatisticsByJobRole(JobRole jobRole);
    Task Create(Employee employee);
    Task Update(long id, Employee employee);
    Task UpdateSalary(long id, decimal salary);
    Task Delete(long id);
}