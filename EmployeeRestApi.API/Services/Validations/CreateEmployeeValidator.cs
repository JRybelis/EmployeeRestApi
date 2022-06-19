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
            .WithErrorCode("Mandatory field missing");
        
        RuleFor(empl => empl.LastName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Mandatory field missing");
        
        RuleFor(empl => empl.BirthDate)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Mandatory field missing");

        RuleFor(empl => empl.BirthDate.Date.Year)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Mandatory field missing");

        RuleFor(empl => empl.StartDate)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Mandatory field missing");

        RuleFor(empl => empl.CurrentSalary)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Mandatory field missing");

        RuleFor(empl => empl.HomeAddress)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Mandatory field missing");
    }
}