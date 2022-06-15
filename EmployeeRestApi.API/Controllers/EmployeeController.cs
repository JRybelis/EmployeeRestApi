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
    public async Task<IEnumerable<EmployeeDto>> GetAll()
    {
        var employees = await _employeeRepository.GetAll();
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }
    
    [Route("getAll/byManager/{id:long}")]
    [HttpGet]
    public async Task<IEnumerable<EmployeeDto>> GetAllByManagerId(long id)
    {
        var employees = await _employeeRepository.GetAllByManagerId(id);
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }
    
    [Route("get/{id:long}")]
    [HttpGet]
    public async Task<EmployeeDto> GetById(long id)
    {
        var employee = await _employeeRepository.GetById(id);
        return _mapper.Map<EmployeeDto>(employee);
    }
    
    [Route("getAll/byName&BirthDateInterval")]
    [HttpGet]
    public async Task<IEnumerable<EmployeeDto>> GetByNameAndBirthDateInterval(string lastName
        , DateTime birthDateRangeMin, DateTime birthDateRangeMax)
    {
        var employees
            = await _employeeRepository.GetByNameAndBirthdateInterval(lastName, birthDateRangeMin
                , birthDateRangeMax);
        return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
    }
    
    [Route("getStatistics/byRole")]
    [HttpGet]
    public async Task<EmployeeStatisticsByRole> GetStatisticsByJobRole(JobRole jobRole)
    {
        return await _employeeRepository.GetStatisticsByJobRole(jobRole);
    }
}