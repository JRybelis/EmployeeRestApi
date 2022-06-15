using EmployeeRestApi.Interfaces;
using EmployeeRestApiLibrary.Enumerations;
using EmployeeRestApiLibrary.Models;

namespace EmployeeRestApi.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    public Task<List<Employee>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<Employee> GetById(long id)
    {
        throw new NotImplementedException();
    }
    
    public Task<List<Employee>> GetAllByManagerId(long id)
    {
        throw new NotImplementedException();
    }

    public Task<Employee> GetByNameAndBirthdateInterval(string LastName, DateTime birthDateRangeMin
        , DateTime birthDateRangeMax)
    {
        throw new NotImplementedException();
    }

    public Task<EmployeeStatisticsByRole> GetStatisticsByJobRole(JobRole jobRole)
    {
        throw new NotImplementedException();
    }

    public Task Create(Employee employee)
    {
        throw new NotImplementedException();
    }

    public Task Update(long id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateSalary(long id, decimal salary)
    {
        throw new NotImplementedException();
    }

    public Task Delete(long id)
    {
        throw new NotImplementedException();
    }
}