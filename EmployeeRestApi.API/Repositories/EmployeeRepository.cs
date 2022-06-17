using EmployeeRestApi.Data;
using EmployeeRestApi.Interfaces;
using EmployeeRestApiLibrary.Enumerations;
using EmployeeRestApiLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeRestApi.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly DataContext _context;
    public static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(
        builder =>
        {
            builder
                // add console as logging target
                .AddConsole()
                // add debug output as logging target
                .AddDebug()
                // set minimum level to log
                .SetMinimumLevel(LogLevel.Debug);
        });

    public EmployeeRepository(DataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    public async Task<IEnumerable<Employee>> GetAll()
    {
        return await _context.Set<Employee>().ToListAsync();
    }
    
    public async Task<IEnumerable<Employee>> GetAllByManagerId(long id)
    {
        var allEmployees = await GetAll();

        return allEmployees.Where(employee => employee.ManagerId == id).ToList();
    }

    public async Task<Employee> GetById(long id)
    {
        var employee = await _context.Set<Employee>().FirstOrDefaultAsync(empl => empl.Id == id);

        if (employee is null)
        {
            throw new ArgumentNullException(nameof(employee));
        }

        return employee;
    }

    public async Task<Employee> GetByNameAndBirthdateInterval(string lastName, DateTime birthDateRangeMin
        , DateTime birthDateRangeMax)
    {
        var employee = await _context.Set<Employee>().FirstOrDefaultAsync(empl =>
            empl.LastName == lastName && 
            empl.BirthDate > birthDateRangeMin && 
            empl.BirthDate < birthDateRangeMax);
        
        if (employee is null)
        {
            throw new ArgumentNullException(nameof(employee));
        }

        return employee;
    }

    public async Task<EmployeeStatisticsByRole> GetStatisticsByJobRole(JobRole jobRole)
    {
        var employeeStatisticsByRole = new EmployeeStatisticsByRole();
        var employeesInThisRole = new List<Employee>();
        var allEmployees = await GetAll();
        decimal totalSalary = 0;
        
        foreach (var employee in allEmployees)
        {
            if (employee.Role != jobRole) continue;
            
            employeesInThisRole.Add(employee);
            totalSalary += employee.CurrentSalary;
        }
        employeeStatisticsByRole.JobRole = jobRole;
        employeeStatisticsByRole.EmployeeCount = employeesInThisRole.Count();
        employeeStatisticsByRole.SalaryAverage = totalSalary/employeeStatisticsByRole.EmployeeCount;

        return employeeStatisticsByRole;
    }

    public async Task<Employee> Create(Employee employee)
    {
        await _context.AddAsync(employee);
        await _context.SaveChangesAsync();
        return employee;
    }

    public async Task Update(long id, Employee employee)
    {
        var existingEmployee = await GetById(id);

        _context.Update(existingEmployee);
        existingEmployee.FirstName = employee.FirstName;
        existingEmployee.LastName = employee.LastName;
        existingEmployee.BirthDate = employee.BirthDate;
        existingEmployee.EmploymentCommencementDate = employee.EmploymentCommencementDate;
        existingEmployee.HomeAddress = employee.HomeAddress;
        existingEmployee.ManagerId = employee.ManagerId;
        existingEmployee.Role = employee.Role;
        existingEmployee.CurrentSalary = employee.CurrentSalary;

        await _context.SaveChangesAsync();
    }

    public async Task UpdateSalary(long id, decimal salary)
    {
        var existingEmployee = await GetById(id);
        
        _context.Update(existingEmployee);
        existingEmployee.CurrentSalary = salary;
        
        await _context.SaveChangesAsync();
    }

    public async Task Delete(long id)
    {
        var employee = await GetById(id);

        if (employee is not null)
        {
            _context.Remove(employee);
        }

        await _context.SaveChangesAsync();
    }
}