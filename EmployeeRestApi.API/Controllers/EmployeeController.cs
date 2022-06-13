using EmployeeRestApi.Interfaces;
using EmployeeRestApiLibrary.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeRestApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : Controller
{
    private readonly ILogger _logger;
    private readonly ILoggerFactory _loggerFactory = new LoggerFactory();
    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _logger = _loggerFactory.CreateLogger(nameof(EmployeeController));
    }

    [HttpGet]
    public async Task<IEnumerable<EmployeeDto>> GetAll()
    {
        Thread.SpinWait(2);
        return new List<EmployeeDto>();
    }
}