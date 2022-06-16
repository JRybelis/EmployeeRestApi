using System.Globalization;
using EmployeeRestApi.Controllers;
using EmployeeRestApi.Data;
using EmployeeRestApi.Interfaces;
using EmployeeRestApi.Repositories;
using EmployeeRestApiLibrary.Enumerations;
using EmployeeRestApiLibrary.Models;
using EmployeeRestApiUnitTests.Validators;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeRestApiUnitTests.Controllers;

public class TestEmployeeController : BaseTest
{
    /*[Test]
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
    }*/

    [Test]
    public Task CreateEmployeeAsync_AttemptToAddASecondCeo()
    {
        // Arrange
        var employeeCeoAddress = new Address
        {
            Street = "Street1-1",
            City = "City1",
            PostCode = "Lt-00001"
        };
        var employee2Address = new Address
        {
            Street = "Street1-2",
            City = "City1",
            PostCode = "Lt-00002"
        };

        var employeeCeo = new Employee
        {
            Id = 1,
            FirstName = "Empl1Forename",
            LastName = "Empl1Surname",
            BirthDate = DateTime.ParseExact("1946/12/14 00:00:00", "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture),
            EmploymentCommencementDate = DateTime.ParseExact("2000/10/01 00:00:00", "yyyy/MM/dd HH:mm:ss",
                CultureInfo.InvariantCulture),
            Manager = null,
            HomeAddress = employeeCeoAddress,
            CurrentSalary = 160000,
            Role = JobRole.ChiefExecutiveOfficer
        };
        
        var employee2 = new Employee
        {
            Id = 2,
            FirstName = "Empl2Forename",
            LastName = "Empl2Surname",
            BirthDate = DateTime.ParseExact("1964/02/44 00:00:00", "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture),
            EmploymentCommencementDate = DateTime.ParseExact("2020/11/11 00:00:00", "yyyy/MM/dd HH:mm:ss",
                CultureInfo.InvariantCulture),
            Manager = null,
            HomeAddress = employee2Address,
            CurrentSalary = 360000,
            Role = JobRole.ChiefExecutiveOfficer
        };
        
        const string errorMessage = "There should be one CEO only and this position should have no managers.";
        employeeCeo = new EmployeeRepository(context).Create(employeeCeo).Result;
        // Act 
        var validationResult = new CreateEmployeeValidator().Validate(employee2);
        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Errors.Single().ErrorMessage, Is.EqualTo(errorMessage));
        });
        
        return Task.CompletedTask;
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