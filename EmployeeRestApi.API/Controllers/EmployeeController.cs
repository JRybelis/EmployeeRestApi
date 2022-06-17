using AutoMapper;
using EmployeeRestApi.Interfaces;
using EmployeeRestApi.Mappings;
using EmployeeRestApiLibrary.Dtos;
using EmployeeRestApiLibrary.Enumerations;
using EmployeeRestApiLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRestApi.Controllers;

[ApiController]
[Route("employees")]
public class EmployeeController : Controller
{
    private readonly ILogger _logger;
    private readonly ILoggerFactory _loggerFactory = new LoggerFactory();
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeValidationService _employeeValidationService;
    // private readonly IMapper _mapper;
    
    public EmployeeController(IEmployeeRepository employeeRepository/*, IMapper mapper*/
        , IEmployeeValidationService employeeValidationService)
    {
        _employeeRepository = employeeRepository;
        // _mapper = mapper;
        _employeeValidationService = employeeValidationService;
        _logger = _loggerFactory.CreateLogger(nameof(EmployeeController));
    }

    [Route("getAll")]
    [HttpGet]
    public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
    {
        var employees = await _employeeRepository.GetAll();
        //return _mapper.Map<IEnumerable<EmployeeDto>>(employees);

        return employees.Select(employee => employee.AsDto()).ToList();
    }
    
    [Route("getAll/byManager/{id:long}")]
    [HttpGet]
    public async Task<IEnumerable<EmployeeDto>> GetAllByManagerIdAsync(long id)
    {
        var employees = await _employeeRepository.GetAllByManagerId(id);
        //return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        
        return employees.Select(employee => employee.AsDto()).ToList();
    }
    
    [Route("getAll/byName&BirthDateInterval")]
    [HttpGet]
    public async Task<EmployeeDto> GetByNameAndBirthDateIntervalAsync(string lastName
        , DateTime birthDateRangeMin, DateTime birthDateRangeMax)
    {
        var employee
            = await _employeeRepository.GetByNameAndBirthdateInterval(lastName, birthDateRangeMin
                , birthDateRangeMax);
        //return _mapper.Map<IEnumerable<EmployeeDto>>(employees);

        return employee.AsDto();
    }
    
    [Route("get/{id:long}")]
    [HttpGet]
    public async Task<EmployeeDto> GetByIdAsync(long id)
    {
        var employee = await _employeeRepository.GetById(id);
        //return _mapper.Map<EmployeeDto>(employee);
        return employee.AsDto();
    }
    
    [Route("getStatistics/byRole")]
    [HttpGet]
    public async Task<EmployeeStatisticsByRole> GetStatisticsByJobRoleAsync(JobRole jobRole)
    {
        return await _employeeRepository.GetStatisticsByJobRole(jobRole);
    }

    [Route("create/employee")]
    [HttpPost]
    public async Task<IActionResult> CreateEmployeeAsync([FromBody] EmployeeDto employeeDto)
    {
        var validationSuccess = await _employeeValidationService.IsDtoValidationSuccess(employeeDto, _employeeRepository);
        if (validationSuccess)
        {
            var employee = /*_mapper.Map<Employee>(employeeDto);*/ employeeDto.AsEntity();
            await _employeeRepository.Create(employee);
            
            var updatedResource = await GetByIdAsync(employee.Id); 
            var actionName = nameof(GetByIdAsync);
            var routeValues = new {id = employee.Id};
            return CreatedAtAction(actionName, routeValues, updatedResource);    
        }

        const string errorMessage = $"Validation of {nameof(employeeDto)} data failed. Database import aborted. Please review the error information above, rectify the data supplied and try again.";
        _logger.LogError(errorMessage);

        return BadRequest(nameof(CreateEmployeeAsync));
    }

    [Route("update/employee/{id:long}")]
    [HttpPut]
    public async Task <IActionResult> UpdateEmployeeAsync([FromRoute] long id, [FromBody] EmployeeDto employeeDto)
    {
        var validationSuccess = await _employeeValidationService.IsDtoValidationSuccess(employeeDto, _employeeRepository, id);
        if (validationSuccess)
        {
            var employee = /*_mapper.Map<Employee>(employeeDto);*/ employeeDto.AsEntity();
            await _employeeRepository.Update(id, employee);

            var updatedResource = await GetByIdAsync(id); 
            var actionName = nameof(GetByIdAsync);
            var routeValues = new {id};

            return AcceptedAtAction(actionName, routeValues, updatedResource);
        }
        
        var errorMessage = $"Validation of {nameof(employeeDto)} data failed. Database update for supplied employee id:{id} aborted. Please review the error information above, rectify the data supplied and try again.";
        _logger.LogError(errorMessage);

        return BadRequest(nameof(UpdateEmployeeAsync));
    }   

    [Route("update/employee/salary/{id:long}")]
    [HttpPut]
    public async Task<IActionResult> UpdateSalaryAsync([FromRoute] long id, [FromBody] decimal salary)
    {
        var validationSuccess
            = await _employeeValidationService.IsCurrentSalaryAPositiveAmount(salary);
        if (validationSuccess)
        {
            var employee = await _employeeRepository.GetById(id);
            await _employeeRepository.UpdateSalary(id, salary);
            
            var updatedResource = await GetByIdAsync(id); 
            var actionName = nameof(GetByIdAsync);
            var routeValues = new {id};
        
            return AcceptedAtAction(actionName, routeValues, updatedResource);    
        }
        
        var errorMessage = $"Validation of the provided salary data failed. Database update for supplied employee id:{id} aborted. Please review the error information above, rectify the data supplied and try again.";
        _logger.LogError(errorMessage);

        return BadRequest(nameof(UpdateSalaryAsync));
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