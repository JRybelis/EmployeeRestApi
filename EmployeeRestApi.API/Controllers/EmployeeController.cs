using System.Web.Http;
using AutoMapper;
using EmployeeRestApi.Interfaces;
using EmployeeRestApiLibrary.Dtos;
using EmployeeRestApiLibrary.Enumerations;
using EmployeeRestApiLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRestApi.Controllers;

[ApiController]
// [Route("[controller]")]
[RoutePrefix("employees")]
public class EmployeeController : Controller
{
    private readonly ILogger _logger;
    private readonly ILoggerFactory _loggerFactory = new LoggerFactory();
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeValidationService _employeeValidationService;
    private readonly IMapper _mapper;
    
    public EmployeeController(IEmployeeRepository employeeRepository, IMapper mapper
        , IEmployeeValidationService employeeValidationService)
    {
        _employeeRepository = employeeRepository;
        _mapper = mapper;
        _employeeValidationService = employeeValidationService;
        _logger = _loggerFactory.CreateLogger(nameof(EmployeeController));
    }

    [Microsoft.AspNetCore.Mvc.Route("getAll")]
    [Microsoft.AspNetCore.Mvc.HttpGet]
    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await _employeeRepository.GetAll();
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }
    
    [Microsoft.AspNetCore.Mvc.Route("getAll/byManager/{id:long}")]
    [Microsoft.AspNetCore.Mvc.HttpGet]
    public async Task<IEnumerable<EmployeeDto>> GetAllByManagerIdAsync(long id)
    {
        var employees = await _employeeRepository.GetAllByManagerId(id);
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }
    
    [Microsoft.AspNetCore.Mvc.Route("getAll/byName&BirthDateInterval")]
    [Microsoft.AspNetCore.Mvc.HttpGet]
    public async Task<IEnumerable<EmployeeDto>> GetByNameAndBirthDateIntervalAsync(string lastName
        , DateTime birthDateRangeMin, DateTime birthDateRangeMax)
    {
        var employees
            = await _employeeRepository.GetByNameAndBirthdateInterval(lastName, birthDateRangeMin
                , birthDateRangeMax);
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }
    
    [Microsoft.AspNetCore.Mvc.Route("get/{id:long}")]
    [Microsoft.AspNetCore.Mvc.HttpGet]
    public async Task<EmployeeDto> GetByIdAsync(long id)
    {
        var employee = await _employeeRepository.GetById(id);
        return _mapper.Map<EmployeeDto>(employee);
    }
    
    [Microsoft.AspNetCore.Mvc.Route("getStatistics/byRole")]
    [Microsoft.AspNetCore.Mvc.HttpGet]
    public async Task<EmployeeStatisticsByRole> GetStatisticsByJobRoleAsync(JobRole jobRole)
    {
        return await _employeeRepository.GetStatisticsByJobRole(jobRole);
    }

    [Microsoft.AspNetCore.Mvc.Route("create/employee")]
    [Microsoft.AspNetCore.Mvc.HttpPost]
    public async Task<IActionResult> CreateEmployeeAsync([Microsoft.AspNetCore.Mvc.FromBody] EmployeeDto employeeDto)
    {
        var validationSuccess = await _employeeValidationService.IsDtoValidationSuccess(employeeDto, _employeeRepository);
        if (validationSuccess)
        {
            var employee = _mapper.Map<Employee>(employeeDto);
            await _employeeRepository.Create(employee);
            return Accepted(nameof(CreateEmployeeAsync));
        }

        const string errorMessage = $"Validation of {nameof(employeeDto)} data failed. Database import aborted. Please review the error information above, rectify the data supplied and try again.";
        _logger.LogError(errorMessage);

        return BadRequest(nameof(CreateEmployeeAsync));
    }

    [Microsoft.AspNetCore.Mvc.Route("update/employee/{id:long}")]
    [Microsoft.AspNetCore.Mvc.HttpPut]
    public async Task <IActionResult> UpdateEmployeeAsync([FromRoute] long id, [Microsoft.AspNetCore.Mvc.FromBody] EmployeeDto employeeDto)
    {
        var validationSuccess = await _employeeValidationService.IsDtoValidationSuccess(employeeDto, _employeeRepository);
        if (validationSuccess)
        {
            var employee = _mapper.Map<Employee>(employeeDto);
            await _employeeRepository.Update(id, employee);
        
            return AcceptedAtAction(nameof(UpdateEmployeeAsync), new {id = employee.Id}, employeeDto);    
        }
        
        var errorMessage = $"Validation of {nameof(employeeDto)} data failed. Database update for supplied employee id:{id} aborted. Please review the error information above, rectify the data supplied and try again.";
        _logger.LogError(errorMessage);

        return BadRequest(nameof(UpdateEmployeeAsync));
    }   

    [Microsoft.AspNetCore.Mvc.Route("update/employee/salary/{id:long}")]
    [Microsoft.AspNetCore.Mvc.HttpPut]
    public async Task<IActionResult> UpdateSalaryAsync([FromRoute] long id, [Microsoft.AspNetCore.Mvc.FromBody] decimal salary)
    {
        var validationSuccess
            = await _employeeValidationService.IsCurrentSalaryAPositiveAmount(salary);
        if (validationSuccess)
        {
            var employee = await _employeeRepository.GetById(id);
            await _employeeRepository.UpdateSalary(id, salary);
        
            return AcceptedAtAction(nameof(UpdateSalaryAsync), new {id = employee.Id}, salary);    
        }
        
        var errorMessage = $"Validation of the provided salary data failed. Database update for supplied employee id:{id} aborted. Please review the error information above, rectify the data supplied and try again.";
        _logger.LogError(errorMessage);

        return BadRequest(nameof(UpdateSalaryAsync));
    }

    [Microsoft.AspNetCore.Mvc.Route("delete/employee/{id:long}")]
    [Microsoft.AspNetCore.Mvc.HttpDelete]
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