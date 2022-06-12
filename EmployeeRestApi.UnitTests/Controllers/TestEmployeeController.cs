using EmployeeRestApi.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeRestApiUnitTests.Controllers;

public class TestEmployeeController : BaseTest
{
    [Test]
    public async Task GetAll_OnSuccess_ReturnsStatusCode200()
    {
        // Arrange
        var mockEmployeeService = new Mock<IEmployeeService>();
        
        mockEmployeeService
            .Setup(service => service.GetAllEmployees())
            .ReturnsAsync(EmployeesFixture.GetTestEmployees);

        var sut = new EmployeeController(mockEmployeeService.Object);

        // Act
        var result = (OkObjectResult) await sut.GetAll();
        
        // Assert
        result.StatusCode.Should().Be(200);
    }

    [Test]
    public void EmployeeFirstName_Always_CannotBeUsedAsLastName()
    {
        // Arrange
        var employee = new Employee()
        {
            FirstName = "Forename",
            LastName = "Surname"
        };

        // Act
        var validationResult = new EmployeeCreateValidator().Validate(Employee);
        
        // Assert
        Assert.True(validationResult.IsValid);
        
        if (!validationResult.IsValid) return;

        Employee = new EmployeeRepository(Context).Post(employee);
        
        Assert.AreNotEqual(employee.FirstName, employee.LastName);
    }
}