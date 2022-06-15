using EmployeeRestApi.Controllers;
using EmployeeRestApi.Interfaces;
using EmployeeRestApiLibrary.Models;
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
        var mockEmployeeRepository = new Mock<IEmployeeRepository>();
        
        mockEmployeeRepository
            .Setup(service => service.GetAll())
            .ReturnsAsync(EmployeesFixture.GetTestEmployees); // reik mocked auto repository ipaisyt cia

        var sut = new EmployeeController(mockEmployeeRepository.Object);

        // Act
        var result = (OkObjectResult) await sut.GetAll();
        
        // Assert
        result.StatusCode.Should().Be(200);
    }

    /*[Test]
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
    }*/
}