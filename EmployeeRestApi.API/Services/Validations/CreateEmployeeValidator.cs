using System.Globalization;
using EmployeeRestApiLibrary.Enumerations;
using EmployeeRestApiLibrary.Models;
using FluentValidation;

namespace EmployeeRestApi.Services.Validations;

public class CreateEmployeeValidator : AbstractValidator<Employee>
{
    public CreateEmployeeValidator()
    {
        RuleFor(empl => empl.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Mandatory field missing")
            .NotEqual(empl => empl.LastName)
            .WithErrorCode("First name cannot be the same as last name.");
        
        RuleFor(empl => empl.LastName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Mandatory field missing")
            .NotEqual(empl => empl.FirstName)
            .WithErrorCode("Last name cannot be the same as first name.");
        
        RuleFor(empl => empl.BirthDate)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Mandatory field missing");

        RuleFor(empl => empl.BirthDate.Date.Year)
            .GreaterThanOrEqualTo(DateTime.Today.Date.Year - 70)
            .WithErrorCode("Age cannot be over 70 years old.")
            .LessThanOrEqualTo(DateTime.Today.Date.Year - 18)
            .WithErrorCode("Age cannot be under 18 years old.");

        RuleFor(empl => empl.EmploymentCommencementDate)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Mandatory field missing")
            .GreaterThanOrEqualTo(DateTime.ParseExact("2000-01-01 00:00:00", "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture))
            .WithErrorCode("Employment commencement date cannot be earlier than 2000-01-01.")
            .LessThanOrEqualTo(DateTime.Today)
            .WithErrorCode("Employment commencement date cannot be in the future.");

        RuleFor(empl => empl.CurrentSalary)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Mandatory field missing")
            .GreaterThanOrEqualTo(0)
            .WithErrorCode("Salary cannot be a negative amount.");

        RuleFor(empl => empl)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Mandatory field missing")
            .When(empl => empl.Role == JobRole.ChiefExecutiveOfficer).Must(empl => empl.ManagerId == null)
            .WithErrorCode("The CEO position should have no managers.");

        RuleFor(empl => empl.HomeAddress)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Mandatory field missing");
    }
}