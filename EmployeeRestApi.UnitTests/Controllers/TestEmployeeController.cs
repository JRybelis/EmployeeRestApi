using EmployeeRestApi.Controllers;
using EmployeeRestApi.Interfaces;
using EmployeeRestApi.Mappings;
using EmployeeRestApiLibrary.Dtos;
using EmployeeRestApiLibrary.Enumerations;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeRestApiUnitTests.Controllers;

public class TestEmployeeController : BaseTest
{
    private Mock<IEmployeeRepository> _employeeRepository;
    private Mock<IEmployeeValidationService> _employeeValidationService;
    private Mock<IValidator<EmployeeDto>> _employeeDtoValidator;

    #region Create employee tests

    [Test]
    public async Task CreateEmployeeAsync_Success_ReturnsHttpStatusCode201()
    {
        // Arrange
        var employeeDto = new EmployeeDto
        {
            FirstName = "TestForename1"
            , LastName = "TestSurname1"
            , BirthDate = DateTime.Parse("1997-7-11 00:00:00")
            , StartDate = DateTime.Parse("2010-7-12 00:00:00")
            , ManagerId = 1
            , HomeAddress = "Some Test Address 1"
            , CurrentSalary = 5000
            , Role = JobRole.ManagerLine
        };

        var employee = employeeDto.AsEntity();
        employee.Id = 1;
        
        _employeeRepository = new Mock<IEmployeeRepository>();
        _employeeDtoValidator = new Mock<IValidator<EmployeeDto>>();
        _employeeValidationService = new Mock<IEmployeeValidationService>();
        
        _employeeRepository.Setup(er => er.Create(employee)).ReturnsAsync(employee);
        _employeeDtoValidator.Setup(edv => edv.ValidateAsync(employeeDto, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());
        _employeeValidationService
            .Setup(evs => evs.IsDtoValidationSuccess(employeeDto, _employeeRepository.Object))
            .ReturnsAsync(true);

        var sut = new EmployeeController(_employeeRepository.Object
            , _employeeValidationService.Object, _employeeDtoValidator.Object);

        // Act
        var result = await sut.CreateEmployeeAsync(employeeDto);
        var createdAtActionResult = result as CreatedAtActionResult;
        
        // Assert
        Assert.That(createdAtActionResult, Is.Not.Null);
        Assert.That(createdAtActionResult.StatusCode, Is.EqualTo(201));
    }

    [Test]
    public async Task CreateEmployeeAsync_Failure_ReturnsHttpStatusCode400()
    {
        // Arrange
        var employeeDto = new EmployeeDto
        {
            FirstName = ""
            , LastName = ""
            , BirthDate = DateTime.Parse("1997-7-11 00:00:00")
            , StartDate = DateTime.Parse("2010-7-12 00:00:00")
            , ManagerId = null
            , HomeAddress = ""
            , CurrentSalary = 2000
            , Role = JobRole.DeveloperGame
        };

        var employee = employeeDto.AsEntity();
        employee.Id = 1;
        
        _employeeRepository = new Mock<IEmployeeRepository>();
        _employeeDtoValidator = new Mock<IValidator<EmployeeDto>>();
        _employeeValidationService = new Mock<IEmployeeValidationService>();
        
        _employeeRepository.Setup(er => er.Create(employee)).ReturnsAsync(employee);
        _employeeDtoValidator.Setup(edv =>
            edv.ValidateAsync(employeeDto, It.IsAny<CancellationToken>())).ReturnsAsync(new ValidationResult());
        _employeeValidationService
            .Setup(evs => evs.IsDtoValidationSuccess(employeeDto, _employeeRepository.Object))
            .ReturnsAsync(false);

        var sut = new EmployeeController(_employeeRepository.Object
            , _employeeValidationService.Object, _employeeDtoValidator.Object);

        // Act
        var result = await sut.CreateEmployeeAsync(employeeDto);
        var badRequestResult = result as BadRequestObjectResult;
        
        // Assert
        Assert.That(badRequestResult, Is.Not.Null);
        Assert.That(badRequestResult.StatusCode, Is.EqualTo(400));
    }
    #endregion

}