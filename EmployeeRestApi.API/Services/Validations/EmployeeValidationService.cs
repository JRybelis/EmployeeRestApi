using System.Globalization;
using EmployeeRestApi.Interfaces;
using EmployeeRestApiLibrary.Dtos;
using EmployeeRestApiLibrary.Enumerations;

namespace EmployeeRestApi.Services;

public class EmployeeValidationService : IEmployeeValidationService
{
    private readonly ILogger _logger;
    private readonly ILoggerFactory _loggerFactory = new LoggerFactory();

    public EmployeeValidationService()
    {
        _logger = _loggerFactory.CreateLogger(nameof(EmployeeValidationService));
    }

    #region Dto validation for create method

    public async Task<bool> IsDtoValidationSuccess(EmployeeDto employeeDto, IEmployeeRepository repository)
    {
        string errorMessage;
        
        #region CEO check

        var allEmployees = await repository.GetAll();
        var ceoEmployee = allEmployees.FirstOrDefault(empl => empl.Role == JobRole.ChiefExecutiveOfficer);
        var isFormSuppliedEmployeeCeo = await IsJobRoleTheCeo(employeeDto.Role);
        
        switch (isFormSuppliedEmployeeCeo)
        {
            case true when employeeDto.ManagerId is not null:
                errorMessage = "The CEO cannot have any managers.";
                _logger.LogDebug(errorMessage);
                return false;
            case true when ceoEmployee is null:
                break;
            case true when employeeDto.BirthDate != ceoEmployee.BirthDate &&
                           employeeDto.LastName != ceoEmployee.LastName &&
                           employeeDto.FirstName != ceoEmployee.FirstName &&
                           employeeDto.EmploymentCommencementDate !=
                           ceoEmployee.EmploymentCommencementDate:
                errorMessage
                    = "There is already a CEO active in the company's organisational structure. There can only be one CEO of the company.";
                _logger.LogDebug(errorMessage);
                return false;
        }

        #endregion

        #region Name check

        if (await IsFirstNameAndLastNameTheSame(employeeDto.FirstName, employeeDto.LastName))
        {
            errorMessage
                = "The employee surname cannot be the same as his or her forename.";
            _logger.LogDebug(errorMessage);
            return false;
        }

        #endregion

        #region Age check

        if (!await IsEmployeeBetweenTheAgesOf18And70(employeeDto.BirthDate))
        {
            errorMessage
                = "The employee cannot be younger than 18 or older than 70 years old.";
            _logger.LogDebug(errorMessage);
            return false;
        }

        #endregion

        #region Start date check

        if (!await IsEmploymentCommencementDateLaterThan20000101(employeeDto
                .EmploymentCommencementDate))
        {
            errorMessage
                = "The employee cannot have started to work for this company prior to 2000-01-01.";
            _logger.LogDebug(errorMessage);
            return false;
        }

        if (await IsEmploymentCommencementDateLaterThanPresent(employeeDto.EmploymentCommencementDate))
        {
            errorMessage
                = "The employee can only be registered as a current employee if his or her employment commencement date is not in the future.";
            _logger.LogDebug(errorMessage);
            return false;
        }

        #endregion

        #region Salary check

        if (!await IsCurrentSalaryAPositiveAmount(employeeDto.CurrentSalary))
        {
            errorMessage
                = "The employee cannot have a salary that is a negative amount.";
            _logger.LogDebug(errorMessage);
            return false;
        }

        #endregion

        return true;
    }

    #endregion
    #region Dto validation for update method

    public async Task<bool> IsDtoValidationSuccess(EmployeeDto employeeDto, IEmployeeRepository repository, long id)
    {
        string errorMessage;

        #region CEO check

        var allEmployees = await repository.GetAll();
        var ceoEmployee = allEmployees.FirstOrDefault(empl => empl.Role == JobRole.ChiefExecutiveOfficer);
        var isFormSuppliedEmployeeCeo = await IsJobRoleTheCeo(employeeDto.Role);
        
        switch (isFormSuppliedEmployeeCeo)
        {
            case true when ceoEmployee is null:
                break;
            case true when ceoEmployee.Id == id:
                break;
            case true when employeeDto.ManagerId is not null:
                errorMessage = "The CEO cannot have any managers.";
                _logger.LogDebug(errorMessage);
                return false;
            case true when employeeDto.BirthDate == ceoEmployee.BirthDate &&
                           employeeDto.LastName == ceoEmployee.LastName &&
                           employeeDto.FirstName == ceoEmployee.FirstName &&
                           employeeDto.EmploymentCommencementDate ==
                           ceoEmployee.EmploymentCommencementDate:
                errorMessage
                    = "There is already a CEO active in the company's organisational structure. There can only be one CEO of the company.";
                _logger.LogDebug(errorMessage);
                return false;
        }

        #endregion

        #region Name check

        if (await IsFirstNameAndLastNameTheSame(employeeDto.FirstName, employeeDto.LastName))
        {
            errorMessage
                = "The employee surname cannot be the same as his or her forename.";
            _logger.LogDebug(errorMessage);
            return false;
        }

        #endregion

        #region Age check

        if (!await IsEmployeeBetweenTheAgesOf18And70(employeeDto.BirthDate))
        {
            errorMessage
                = "The employee cannot be younger than 18 or older than 70 years old.";
            _logger.LogDebug(errorMessage);
            return false;
        }

        #endregion

        #region Start date check

        if (!await IsEmploymentCommencementDateLaterThan20000101(employeeDto
                .EmploymentCommencementDate))
        {
            errorMessage
                = "The employee cannot have started to work for this company prior to 2000-01-01.";
            _logger.LogDebug(errorMessage);
            return false;
        }

        if (await IsEmploymentCommencementDateLaterThanPresent(employeeDto.EmploymentCommencementDate))
        {
            errorMessage
                = "The employee can only be registered as a current employee if his or her employment commencement date is not in the future.";
            _logger.LogDebug(errorMessage);
            return false;
        }

        #endregion

        #region Salary check

        if (!await IsCurrentSalaryAPositiveAmount(employeeDto.CurrentSalary))
        {
            errorMessage
                = "The employee cannot have a salary that is a negative amount.";
            _logger.LogDebug(errorMessage);
            return false;
        }

        #endregion

        return true;
    }

    #endregion
    

    public Task<bool> IsJobRoleTheCeo(JobRole jobRole)
    {
        return Task.FromResult(jobRole == JobRole.ChiefExecutiveOfficer);
    }

    public Task<bool> IsFirstNameAndLastNameTheSame(string firstName, string lastName)
    {
        return Task.FromResult(string.Equals(firstName, lastName));
    }

    public Task<bool> IsEmployeeBetweenTheAgesOf18And70(DateTime birthDate)
    {
        var birthDateSupplied = birthDate.Date;
        var currentDate = DateTime.Today.Date;
        var ageInYears = (currentDate - birthDateSupplied).TotalDays / 365;

        return Task.FromResult(ageInYears is > 18 and < 70);
    }

    public Task<bool> IsEmploymentCommencementDateLaterThan20000101(DateTime startDate)
    {
        var startDateSupplied = startDate;
        var companyEmployeeRecordsStartDate = DateTime.ParseExact("2000-01-01 00:00:00", "yyyy-MM-dd HH:mm:ss"
            , CultureInfo.InvariantCulture).Date;

        return Task.FromResult(startDateSupplied > companyEmployeeRecordsStartDate);
    }

    public Task<bool> IsEmploymentCommencementDateLaterThanPresent(DateTime startDate)
    {
        var startDateSupplied = startDate;
        var currentDate = DateTime.Today.Date;
        
        return Task.FromResult(startDateSupplied > currentDate);
    }

    public Task<bool> IsCurrentSalaryAPositiveAmount(decimal salary)
    {
        return Task.FromResult(salary > 0);
    }
}