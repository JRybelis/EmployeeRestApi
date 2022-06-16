using EmployeeRestApiLibrary.Models;
using FluentValidation;

namespace EmployeeRestApiUnitTests.Validators;

public class AddressCreateValidator : AbstractValidator<Address>
{
    public AddressCreateValidator()
    {
        RuleFor(adrs => adrs.City)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Mandatory field missing");
        
        RuleFor(adrs => adrs.Street)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Mandatory field missing");
        
        RuleFor(adrs => adrs.PostCode)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithErrorCode("Mandatory field missing");
    }
}