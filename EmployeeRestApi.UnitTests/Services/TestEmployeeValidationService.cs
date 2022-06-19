using System.Globalization;
using EmployeeRestApi.Interfaces;
using EmployeeRestApi.Services;
using EmployeeRestApi.Services.Validations;
using EmployeeRestApiLibrary.Dtos;
using EmployeeRestApiLibrary.Enumerations;
using EmployeeRestApiLibrary.Models;
using EmployeeRestApiUnitTests.Helpers;
using Moq;

namespace EmployeeRestApiUnitTests.Controllers;

public class TestEmployeeValidationService : BaseTest
{
    private Mock<IEmployeeRepository> _employeeRepository;
    private Mock<IEmployeeValidationService> _employeeValidationService;

    #region Mandatory fields check tests

    [TestCase("", "TestSurname", "1997-7-11 00:00:00", "2010-7-12 00:00:00", 1, "Some Test Address 1"
        , 2000, JobRole.Accountant)]
    [TestCase("TestForename", "", "1997-7-11 00:00:00", "2010-7-12 00:00:00", 2, "Some Test Address 1"
        , 2000, JobRole.AdministratorDatabase)]
    [TestCase("TestForename", "TestSurname", "1997-7-11 00:00:00", "2010-7-12 00:00:00", 3, ""
        , 2000, JobRole.Recruiter)]
    [TestCase(null, "TestSurname", "1997-7-11 00:00:00", "2010-7-12 00:00:00", 1, "Some Test Address 1"
        , 2000, JobRole.EngineerSecurity)]
    [TestCase("TestForename", null, "1997-7-11 00:00:00", "2010-7-12 00:00:00", 1, "Some Test Address 1"
        , 2000, JobRole.EngineerSecurity)]
    [TestCase("TestForename", "TestSurname", "1997-7-11 00:00:00", "2010-7-12 00:00:00", 1, null
        , 2000, JobRole.DeveloperGame)]
    [TestCase(null, null, "1997-7-11 00:00:00", "2010-7-12 00:00:00", null, null
        , 2000, JobRole.DeveloperGame)]
    public async Task CreateEmployeeAsync_OmitAddingAnyMandatoryFields_ValidationFails(
        string forename, string surname, DateTime birthDate, DateTime startDate, long? managerId, string address
        , decimal salary, JobRole role)
    {
        const string errorMessage = "Mandatory field missing"; 
        var employee = new Employee
        {
            FirstName = forename
            , LastName = surname
            , BirthDate = birthDate
            , StartDate = startDate
            , ManagerId = managerId
            , HomeAddress = address
            , CurrentSalary = salary
            , Role = role
        };
        
        CheckError(new CreateEmployeeValidator(), errorMessage, employee);
    }
    
    [TestCase("TestForename", "TestSurname", "1997-7-11 00:00:00", "2010-7-12 00:00:00", null, "Some Test Address 1"
        , 2000, JobRole.DeveloperGame)]
    [TestCase("TestForename", "TestSurname", "1997-7-11 00:00:00", "2010-7-12 00:00:00", 1, "Some Test Address 1"
        , 2000, JobRole.DeveloperGame)]
    public async Task CreateEmployeeAsync_NoMandatoryFieldsOmitted_ValidationPasses(
        string forename, string surname, DateTime birthDate, DateTime startDate, long? managerId, string address
        , decimal salary, JobRole role)
    {
        var employee = new Employee
        {
            FirstName = forename
            , LastName = surname
            , BirthDate = birthDate
            , StartDate = startDate
            , ManagerId = managerId
            , HomeAddress = address
            , CurrentSalary = salary
            , Role = role
        };
        
        CheckCorrectOutcome(new CreateEmployeeValidator(), employee);
    }
    #endregion
    
    #region Ceo check tests

    [Test]
    public async Task CreateEmployeeAsync_AddCeoWithManagerId_ValidationFails()
    {
        #region Arrange

        // Arrange
        var listEmployees = new List<Employee>();
        var employeeDto = new EmployeeDto()
        {
            FirstName = "Empl2Forename",
            LastName = "Empl2Surname",
            BirthDate = DateTime.Parse("1964-02-24 00:00:00"),
            StartDate = DateTime.ParseExact("2020-11-11 00:00:00", "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture),
            ManagerId = 1,
            HomeAddress = "Some st. 2, SomeCityB, LT-5005",
            CurrentSalary = 360000,
            Role = JobRole.ChiefExecutiveOfficer
        };
        
        listEmployees.Add(new Employee
        {
            Id = 1,
            FirstName = "TestForename1",
            LastName = "TestSurname1",
            BirthDate = DateTime.Parse("1998-02-12 20:00:00"),
            StartDate = DateTime.Parse("2020-01-01 00:10:00"),
            ManagerId = null,
            HomeAddress = "Some address 1",
            CurrentSalary = 20000,
            Role = JobRole.EngineerServer
        });
        listEmployees.Add(new Employee
        {
            Id = 2,
            FirstName = "TestForename2",
            LastName = "TestSurname2",
            BirthDate = DateTime.Parse("1997-02-12 20:00:00"),
            StartDate = DateTime.Parse("2020-02-01 00:10:00"),
            ManagerId = null,
            HomeAddress = "Some address 2",
            CurrentSalary = 20000,
            Role = JobRole.EngineerDatabase
        });
        listEmployees.Add(new Employee
        {
            Id = 3,
            FirstName = "TestForename3",
            LastName = "TestSurname3",
            BirthDate = DateTime.Parse("1999-02-12 20:00:00"),
            StartDate = DateTime.Parse("2020-05-01 00:10:00"),
            ManagerId = null,
            HomeAddress = "Some address 3",
            CurrentSalary = 21000,
            Role = JobRole.EngineerCloud
        });

        var employee = employeeDto.AsEntity();
        _employeeRepository = new Mock<IEmployeeRepository>();
        _employeeRepository.Setup(er => er.Create(employee)).ReturnsAsync(employee);
        _employeeRepository.Setup(er => er.GetAll()).ReturnsAsync(listEmployees);
        
        var sut = new EmployeeValidationService();

        #endregion
        
        // Act 
        var result = await sut.IsDtoValidationSuccess(employeeDto, _employeeRepository.Object); 
        
        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task CreateEmployeeAsync_AddASecondCeo_ValidationFails()
    {
        // Arrange
        #region Arrange
        var listEmployees = new List<Employee>();
        
        var employeeCeo = new Employee
        {
            Id = 1,
            FirstName = "Empl1Forename",
            LastName = "Empl1Surname",
            BirthDate = DateTime.Parse("1946-12-14 00:00:00"),
            StartDate = DateTime.ParseExact("2000-10-01 00:00:00", "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture),
            ManagerId = null,
            HomeAddress = "Some st. 5, SomeCityA, LT-5555",
            CurrentSalary = 160000,
            Role = JobRole.ChiefExecutiveOfficer
        };
        listEmployees.Add(employeeCeo);
        
        listEmployees.Add(new Employee
        {
            Id = 1,
            FirstName = "TestForename1",
            LastName = "TestSurname1",
            BirthDate = DateTime.Parse("1998-02-12 20:00:00"),
            StartDate = DateTime.Parse("2020-01-01 00:10:00"),
            ManagerId = null,
            HomeAddress = "Some address 1",
            CurrentSalary = 20000,
            Role = JobRole.EngineerServer
        });
        listEmployees.Add(new Employee
        {
            Id = 2,
            FirstName = "TestForename2",
            LastName = "TestSurname2",
            BirthDate = DateTime.Parse("1997-02-12 20:00:00"),
            StartDate = DateTime.Parse("2020-02-01 00:10:00"),
            ManagerId = null,
            HomeAddress = "Some address 2",
            CurrentSalary = 20000,
            Role = JobRole.EngineerDatabase
        });
        listEmployees.Add(new Employee
        {
            Id = 3,
            FirstName = "TestForename3",
            LastName = "TestSurname3",
            BirthDate = DateTime.Parse("1999-02-12 20:00:00"),
            StartDate = DateTime.Parse("2020-05-01 00:10:00"),
            ManagerId = null,
            HomeAddress = "Some address 3",
            CurrentSalary = 21000,
            Role = JobRole.EngineerCloud
        });
        
        var employeeDto = new EmployeeDto()
        {
            FirstName = "Empl2Forename",
            LastName = "Empl2Surname",
            BirthDate = DateTime.Parse/*Exact*/("1964-02-24 00:00:00"/*, "s", CultureInfo.InvariantCulture*/),
            StartDate = DateTime.ParseExact("2020-11-11 00:00:00", "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture),
            ManagerId = null,
            HomeAddress = "Some st. 2, SomeCityB, LT-5005",
            CurrentSalary = 360000,
            Role = JobRole.ChiefExecutiveOfficer
        };
        
        var employee = employeeDto.AsEntity();
        _employeeRepository = new Mock<IEmployeeRepository>();
        _employeeRepository.Setup(er => er.Create(employee)).ReturnsAsync(employee);
        _employeeRepository.Setup(er => er.GetAll()).ReturnsAsync(listEmployees);
        
        var sut = new EmployeeValidationService();
        #endregion
        
        // Act 
        var result = await sut.IsDtoValidationSuccess(employeeDto, _employeeRepository.Object);
        
        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task CreateEmployeeAsync_AddAFirstCeo_ValidationPasses()
    {
        // Arrange
        #region Arrange
        var listEmployees = new List<Employee>();
        
        listEmployees.Add(new Employee
        {
            Id = 1,
            FirstName = "TestForename1",
            LastName = "TestSurname1",
            BirthDate = DateTime.Parse("1998-02-12 20:00:00"),
            StartDate = DateTime.Parse("2020-01-01 00:10:00"),
            ManagerId = null,
            HomeAddress = "Some address 1",
            CurrentSalary = 20000,
            Role = JobRole.EngineerServer
        });
        listEmployees.Add(new Employee
        {
            Id = 2,
            FirstName = "TestForename2",
            LastName = "TestSurname2",
            BirthDate = DateTime.Parse("1997-02-12 20:00:00"),
            StartDate = DateTime.Parse("2020-02-01 00:10:00"),
            ManagerId = null,
            HomeAddress = "Some address 2",
            CurrentSalary = 20000,
            Role = JobRole.EngineerDatabase
        });
        listEmployees.Add(new Employee
        {
            Id = 3,
            FirstName = "TestForename3",
            LastName = "TestSurname3",
            BirthDate = DateTime.Parse("1999-02-12 20:00:00"),
            StartDate = DateTime.Parse("2020-05-01 00:10:00"),
            ManagerId = null,
            HomeAddress = "Some address 3",
            CurrentSalary = 21000,
            Role = JobRole.EngineerCloud
        });
        
        var employeeDto = new EmployeeDto()
        {
            FirstName = "Empl2Forename",
            LastName = "Empl2Surname",
            BirthDate = DateTime.Parse("1964-02-24 00:00:00"),
            StartDate = DateTime.ParseExact("2020-11-11 00:00:00", "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture),
            ManagerId = null,
            HomeAddress = "Some st. 2, SomeCityB, LT-5005",
            CurrentSalary = 360000,
            Role = JobRole.ChiefExecutiveOfficer
        };
        
        var employee = employeeDto.AsEntity();
        _employeeRepository = new Mock<IEmployeeRepository>();
        _employeeRepository.Setup(er => er.Create(employee)).ReturnsAsync(employee);
        _employeeRepository.Setup(er => er.GetAll()).ReturnsAsync(listEmployees);
        
        var sut = new EmployeeValidationService();
        #endregion
        
        // Act 
        var result = await sut.IsDtoValidationSuccess(employeeDto, _employeeRepository.Object);
        
        // Assert
        Assert.That(result, Is.True);
    }

    #endregion

    #region Name check tests
    
    [Test]
    public async Task CreateEmployeeAsync_EmployeeFirstNameSameAsLastName_ValidationFails()
    {
        // Arrange
        #region Arrange
       var employeeDto = new EmployeeDto()
        {
            FirstName = "Empl2Forename",
            LastName = "Empl2Forename",
            BirthDate = DateTime.Parse("1964-02-24 00:00:00"),
            StartDate = DateTime.ParseExact("2020-11-11 00:00:00", "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture),
            ManagerId = null,
            HomeAddress = "Some st. 2, SomeCityB, LT-5005",
            CurrentSalary = 360000,
            Role = JobRole.ChiefExecutiveOfficer
        };
        
        var employee = employeeDto.AsEntity();
        _employeeRepository = new Mock<IEmployeeRepository>();
        _employeeRepository.Setup(er => er.Create(employee)).ReturnsAsync(employee);

        var sut = new EmployeeValidationService();
        #endregion
        
        // Act
        var result = await sut.IsDtoValidationSuccess(employeeDto, _employeeRepository.Object);
        
        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task CreateEmployeeAsync_EmployeeLastNameSameAsFirstName_ValidationFails()
    {
        // Arrange
        #region Arrange
        var employeeDto = new EmployeeDto()
        {
            FirstName = "Empl2Surname",
            LastName = "Empl2Surname",
            BirthDate = DateTime.Parse("1964-02-24 00:00:00"),
            StartDate = DateTime.ParseExact("2020-11-11 00:00:00", "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture),
            ManagerId = null,
            HomeAddress = "Some st. 2, SomeCityB, LT-5005",
            CurrentSalary = 360000,
            Role = JobRole.ChiefExecutiveOfficer
        };
        
        var employee = employeeDto.AsEntity();
        _employeeRepository = new Mock<IEmployeeRepository>();
        _employeeRepository.Setup(er => er.Create(employee)).ReturnsAsync(employee);

        var sut = new EmployeeValidationService();
        #endregion
        
        // Act
        var result = await sut.IsDtoValidationSuccess(employeeDto, _employeeRepository.Object);
        
        // Assert
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task CreateEmployeeAsync_EmployeeFirstNameDistinctFromLastName_ValidationPasses()
    {
        // Arrange
        #region Arrange
        var employeeDto = new EmployeeDto()
        {
            FirstName = "Empl2Forename",
            LastName = "Empl2Surname",
            BirthDate = DateTime.Parse("1964-02-24 00:00:00"),
            StartDate = DateTime.ParseExact("2020-11-11 00:00:00", "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture),
            ManagerId = null,
            HomeAddress = "Some st. 2, SomeCityB, LT-5005",
            CurrentSalary = 360000,
            Role = JobRole.ChiefExecutiveOfficer
        };
        
        var employee = employeeDto.AsEntity();
        _employeeRepository = new Mock<IEmployeeRepository>();
        _employeeRepository.Setup(er => er.Create(employee)).ReturnsAsync(employee);

        var sut = new EmployeeValidationService();
        #endregion
        
        // Act
        var result = await sut.IsDtoValidationSuccess(employeeDto, _employeeRepository.Object);
        
        // Assert
        Assert.That(result, Is.True);
    }
    #endregion

    #region Age check tests

    [Test]
    public async Task CreateEmployeeAsync_EmployeeUnderAllowedAge_ValidationFails()
    {
        // Arrange
        #region Arrange
        var employeeDto = new EmployeeDto()
        {
            FirstName = "Empl2Forename",
            LastName = "Empl2Surname",
            BirthDate = DateTime.Parse("2005-06-16 00:00:00"),
            StartDate = DateTime.ParseExact("2020-11-11 00:00:00", "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture),
            ManagerId = null,
            HomeAddress = "Some st. 2, SomeCityB, LT-5005",
            CurrentSalary = 360000,
            Role = JobRole.ChiefExecutiveOfficer
        };
        
        var employee = employeeDto.AsEntity();
        _employeeRepository = new Mock<IEmployeeRepository>();
        _employeeRepository.Setup(er => er.Create(employee)).ReturnsAsync(employee);

        var sut = new EmployeeValidationService();
        #endregion
        
        // Act
        var result = await sut.IsDtoValidationSuccess(employeeDto, _employeeRepository.Object);
        
        // Assert
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task CreateEmployeeAsync_EmployeeOverAllowedAge_ValidationFails()
    {
        // Arrange
        #region Arrange
        var employeeDto = new EmployeeDto()
        {
            FirstName = "Empl2Forename",
            LastName = "Empl2Surname",
            BirthDate = DateTime.Parse("1951-06-16 00:00:00"),
            StartDate = DateTime.ParseExact("2020-11-11 00:00:00", "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture),
            ManagerId = null,
            HomeAddress = "Some st. 2, SomeCityB, LT-5005",
            CurrentSalary = 360000,
            Role = JobRole.ChiefExecutiveOfficer
        };
        
        var employee = employeeDto.AsEntity();
        _employeeRepository = new Mock<IEmployeeRepository>();
        _employeeRepository.Setup(er => er.Create(employee)).ReturnsAsync(employee);

        var sut = new EmployeeValidationService();
        #endregion
        
        // Act
        var result = await sut.IsDtoValidationSuccess(employeeDto, _employeeRepository.Object);
        
        // Assert
        Assert.That(result, Is.False);
    }
    
    [TestCase("1953-06-16 00:00:00")]
    [TestCase("1962-06-16 00:00:00")]
    [TestCase("1972-06-16 00:00:00")]
    [TestCase("1982-06-16 00:00:00")]
    [TestCase("1992-06-16 00:00:00")]
    [TestCase("1999-06-16 00:00:00")]
    [TestCase("2000-06-16 00:00:00")]
    [TestCase("2002-06-16 00:00:00")]
    [TestCase("2004-06-16 00:00:00")]
    public async Task CreateEmployeeAsync_EmployeeOlderThan18YoungerThan70_ValidationPasses(string birthDate)
    {
        // Arrange
        #region Arrange
        var employeeDto = new EmployeeDto()
        {
            FirstName = "Empl2Forename",
            LastName = "Empl2Surname",
            BirthDate = DateTime.Parse(birthDate),
            StartDate = DateTime.ParseExact("2020-11-11 00:00:00", "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture),
            ManagerId = null,
            HomeAddress = "Some st. 2, SomeCityB, LT-5005",
            CurrentSalary = 360000,
            Role = JobRole.ChiefExecutiveOfficer
        };
        
        var employee = employeeDto.AsEntity();
        _employeeRepository = new Mock<IEmployeeRepository>();
        _employeeRepository.Setup(er => er.Create(employee)).ReturnsAsync(employee);

        var sut = new EmployeeValidationService();
        #endregion
        
        // Act
        var result = await sut.IsDtoValidationSuccess(employeeDto, _employeeRepository.Object);
        
        // Assert
        Assert.That(result, Is.True);
    }
    #endregion

    #region Employment commencement date tests

    [TestCase("1953-06-16 00:00:00")]
    [TestCase("1962-06-16 00:00:00")]
    [TestCase("1972-06-16 00:00:00")]
    [TestCase("1982-06-16 00:00:00")]
    [TestCase("1992-06-16 00:00:00")]
    [TestCase("1999-06-16 00:00:00")]
    [TestCase("1999-12-31 23:59:59")]
    public async Task CreateEmployeeAsync_EmployeeStartedBefore20000101_ValidationFails(string startDate)
    {
        // Arrange

        #region Arrange

        var employeeDto = new EmployeeDto()
        {
            FirstName = "Empl2Forename",
            LastName = "Empl2Surname",
            BirthDate = DateTime.Parse("1972-06-16 00:00:00"),
            StartDate = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture),
            ManagerId = null,
            HomeAddress = "Some st. 2, SomeCityB, LT-5005",
            CurrentSalary = 360000,
            Role = JobRole.ChiefExecutiveOfficer
        };

        var employee = employeeDto.AsEntity();
        _employeeRepository = new Mock<IEmployeeRepository>();
        _employeeRepository.Setup(er => er.Create(employee)).ReturnsAsync(employee);

        var sut = new EmployeeValidationService();

        #endregion

        // Act
        var result = await sut.IsDtoValidationSuccess(employeeDto, _employeeRepository.Object);

        // Assert
        Assert.That(result, Is.False);
    }

    [TestCase("2000-01-01 00:00:01")]
    [TestCase("2000-10-01 00:00:01")]
    [TestCase("2000-12-01 00:00:01")]
    [TestCase("2001-01-01 00:00:01")]
    [TestCase("2002-01-01 00:00:01")]
    [TestCase("2020-01-01 00:00:01")]
    public async Task CreateEmployeeAsync_EmployeeStartedAfter20000101_ValidationPasses(string startDate)
    {
        // Arrange
        #region Arrange
        var employeeDto = new EmployeeDto()
        {
            FirstName = "Empl2Forename",
            LastName = "Empl2Surname",
            BirthDate = DateTime.Parse("1972-06-16 00:00:00"),
            StartDate = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture),
            ManagerId = null,
            HomeAddress = "Some st. 2, SomeCityB, LT-5005",
            CurrentSalary = 360000,
            Role = JobRole.ChiefExecutiveOfficer
        };
    
        var employee = employeeDto.AsEntity();
        _employeeRepository = new Mock<IEmployeeRepository>();
        _employeeRepository.Setup(er => er.Create(employee)).ReturnsAsync(employee);

        var sut = new EmployeeValidationService();
        #endregion
    
        // Act
        var result = await sut.IsDtoValidationSuccess(employeeDto, _employeeRepository.Object);
    
        // Assert
        Assert.That(result, Is.True);
    }
    
    [TestCase("2023-01-01 00:00:01")]
    [TestCase("2024-10-01 00:00:01")]
    [TestCase("2025-12-01 00:00:01")]
    [TestCase("2031-01-01 00:00:01")]
    [TestCase("2022-07-01 00:00:01")]
    [TestCase("2050-01-01 00:00:01")]
    public async Task CreateEmployeeAsync_EmployeeStartedAfterCurrentDate_ValidationFails(string startDate)
    {
        // Arrange
        #region Arrange
        var employeeDto = new EmployeeDto()
        {
            FirstName = "Empl2Forename",
            LastName = "Empl2Surname",
            BirthDate = DateTime.Parse("1972-06-16 00:00:00"),
            StartDate = DateTime.ParseExact(startDate, "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture),
            ManagerId = null,
            HomeAddress = "Some st. 2, SomeCityB, LT-5005",
            CurrentSalary = 360000,
            Role = JobRole.ChiefExecutiveOfficer
        };
    
        var employee = employeeDto.AsEntity();
        _employeeRepository = new Mock<IEmployeeRepository>();
        _employeeRepository.Setup(er => er.Create(employee)).ReturnsAsync(employee);

        var sut = new EmployeeValidationService();
        #endregion
    
        // Act
        var result = await sut.IsDtoValidationSuccess(employeeDto, _employeeRepository.Object);
    
        // Assert
        Assert.That(result, Is.False);
    }

    #endregion

    #region Salary check tests

    [TestCase(20)]
    [TestCase(10)]
    [TestCase(1)]
    [TestCase(0.1)]
    [TestCase(0.01)]
    [TestCase(11111111)]
    [TestCase(400000000)]
    public async Task CreateEmployeeAsync_EmployeeSalaryIsPositive_ValidationPasses(decimal salary)
    {
        // Arrange
        #region Arrange
        var employeeDto = new EmployeeDto()
        {
            FirstName = "Empl2Forename",
            LastName = "Empl2Surname",
            BirthDate = DateTime.Parse("1972-06-16 00:00:00"),
            StartDate = DateTime.ParseExact("2003-01-01 00:00:01", "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture),
            ManagerId = null,
            HomeAddress = "Some st. 2, SomeCityB, LT-5005",
            CurrentSalary = salary,
            Role = JobRole.ChiefExecutiveOfficer
        };
    
        var employee = employeeDto.AsEntity();
        _employeeRepository = new Mock<IEmployeeRepository>();
        _employeeRepository.Setup(er => er.Create(employee)).ReturnsAsync(employee);

        var sut = new EmployeeValidationService();
        #endregion
    
        // Act
        var result = await sut.IsDtoValidationSuccess(employeeDto, _employeeRepository.Object);
    
        // Assert
        Assert.That(result, Is.True);
    }
    
    [TestCase(-20)]
    [TestCase(-10)]
    [TestCase(-1)]
    [TestCase(-0.1)]
    [TestCase(-0.01)]
    [TestCase(-11111111)]
    [TestCase(-400000000)]
    public async Task CreateEmployeeAsync_EmployeeSalaryIsNegative_ValidationFails(decimal salary)
    {
        // Arrange
        #region Arrange
        var employeeDto = new EmployeeDto()
        {
            FirstName = "Empl2Forename",
            LastName = "Empl2Surname",
            BirthDate = DateTime.Parse("1972-06-16 00:00:00"),
            StartDate = DateTime.ParseExact("2003-01-01 00:00:01", "yyyy-MM-dd HH:mm:ss",
                CultureInfo.InvariantCulture),
            ManagerId = null,
            HomeAddress = "Some st. 2, SomeCityB, LT-5005",
            CurrentSalary = salary,
            Role = JobRole.ChiefExecutiveOfficer
        };
    
        var employee = employeeDto.AsEntity();
        _employeeRepository = new Mock<IEmployeeRepository>();
        _employeeRepository.Setup(er => er.Create(employee)).ReturnsAsync(employee);

        var sut = new EmployeeValidationService();
        #endregion
    
        // Act
        var result = await sut.IsDtoValidationSuccess(employeeDto, _employeeRepository.Object);
    
        // Assert
        Assert.That(result, Is.False);
    }

    #endregion
    
}