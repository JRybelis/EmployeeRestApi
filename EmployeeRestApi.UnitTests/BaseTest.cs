using EmployeeRestApiUnitTests.Helpers;
using FluentValidation;

namespace EmployeeRestApiUnitTests;

public class BaseTest : InMemoryDbContextRunner
{
    protected static void CheckError<T>(AbstractValidator<T> validator, string errorMessage
        , T validationObject)
    {
        var validationResult = validator.Validate(validationObject);
        Assert.That(validationResult.IsValid, Is.False);

        if (validationResult.IsValid) return;

        var hasError
            = validationResult.Errors.Any(assert => assert.ErrorCode.Equals(errorMessage));
        Assert.That(hasError, Is.True);
    }
    
    protected static void CheckCorrectOutcome<T>(AbstractValidator<T> validator
        , T validationObject)
    {
        var validationResult = validator.Validate(validationObject);
        Assert.That(validationResult.IsValid, Is.True);
    }
}