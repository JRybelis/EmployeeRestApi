namespace EmployeeRestApiUnitTests.Controllers;

public class TestEmployeeController : BaseTest
{
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