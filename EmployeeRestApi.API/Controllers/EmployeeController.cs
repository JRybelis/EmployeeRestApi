using AutoMapper;
using EmployeeRestApi.Interfaces;
using EmployeeRestApiLibrary.Dtos;
using EmployeeRestApiLibrary.Enumerations;
using EmployeeRestApiLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRestApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : Controller
{
    private readonly ILogger _logger;
    private readonly ILoggerFactory _loggerFactory = new LoggerFactory();
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMapper _mapper;
    
    public EmployeeController(IEmployeeRepository employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
        _logger = _loggerFactory.CreateLogger(nameof(EmployeeController));
    }

    [Route("getAll")]
    [HttpGet]
    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await _employeeRepository.GetAll();
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }
    
    [Route("getAll/byManager/{id:long}")]
    [HttpGet]
    public async Task<IEnumerable<EmployeeDto>> GetAllByManagerIdAsync(long id)
    {
        var employees = await _employeeRepository.GetAllByManagerId(id);
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }
    
    [Route("get/{id:long}")]
    [HttpGet]
    public async Task<EmployeeDto> GetByIdAsync(long id)
    {
        var employee = await _employeeRepository.GetById(id);
        return _mapper.Map<EmployeeDto>(employee);
    }
    
    [Route("getAll/byName&BirthDateInterval")]
    [HttpGet]
    public async Task<IEnumerable<EmployeeDto>> GetByNameAndBirthDateIntervalAsync(string lastName
        , DateTime birthDateRangeMin, DateTime birthDateRangeMax)
    {
        var employees
            = await _employeeRepository.GetByNameAndBirthdateInterval(lastName, birthDateRangeMin
                , birthDateRangeMax);
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }
    
    [Route("getStatistics/byRole")]
    [HttpGet]
    public async Task<EmployeeStatisticsByRole> GetStatisticsByJobRoleAsync(JobRole jobRole)
    {
        return await _employeeRepository.GetStatisticsByJobRole(jobRole);
    }

    [Route("create/employee")]
    [HttpPost]
    public async Task<ActionResult<Employee>> CreateEmployeeAsync([FromBody] EmployeeDto employeeDto)
    {
        var employee = _mapper.Map<Employee>(employeeDto);
        await _employeeRepository.Create(employee);
        
        return CreatedAtRoute(nameof(CreateEmployeeAsync), new {id = employee.Id}, employeeDto);
    }

    [Route("update/employee/{id:long}")]
    [HttpPut]
    public async Task <IActionResult> UpdateEmployeeAsync([FromRoute] long id, [FromBody] EmployeeDto employeeDto)
    {
        var employee = _mapper.Map<Employee>(employeeDto);
        await _employeeRepository.Update(id, employee);
        
        return AcceptedAtAction(nameof(UpdateEmployeeAsync), new {id = employee.Id}, employeeDto);
    }

    [Route("update/employee/salary/{id:long}")]
    [HttpPut]
    public async Task<IActionResult> UpdateSalaryAsync([FromRoute] long id, [FromBody] decimal salary)
    {
        var employee = await _employeeRepository.GetById(id);
        await _employeeRepository.UpdateSalary(id, salary);
        
        return AcceptedAtAction(nameof(UpdateSalaryAsync), new {id = employee.Id}, salary);
    }

    [Route("delete/employee/{id:long}")]
    [HttpDelete]
    public async Task<IActionResult> DeleteEmployeeAsync([FromRoute] long id)
    {
        var existingEmployee = await _employeeRepository.GetById(id);
        
        if (existingEmployee is null)
        {
            return NotFound();
        }

        await _employeeRepository.Delete(id);
        
        return NoContent();
    }
}