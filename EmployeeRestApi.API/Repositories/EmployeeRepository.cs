using EmployeeRestApi.Interfaces;
using EmployeeRestApiLibrary.Dtos;
using EmployeeRestApiLibrary.Models;

namespace EmployeeRestApi.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    public Task<List<EmployeeDto>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<EmployeeDto> GetById(long id)
    {
        throw new NotImplementedException();
    }

    public Task<EmployeeDto> GetByNameAndBirthdateInterval(string FirstName, string LastName, DateTime birthDateRangeMin
        , DateTime birthDateRangeMax)
    {
        throw new NotImplementedException();
    }

    public Task<List<EmployeeDto>> GetAllByManagerId(long id)
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