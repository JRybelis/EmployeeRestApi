using EmployeeRestApiUnitTests.Helpers;
using FluentValidation;

namespace EmployeeRestApiUnitTests;

public class BaseTest : InMemoryDbContextRunner
{
    protected static void CheckError<T>(AbstractValidator<T> validator, string errorMessage
        , T validatedMethod)
    {
        var validationResult = validator.Validate(validatedMethod);
        Assert.That(validationResult.IsValid, Is.False);

        if (validationResult.IsValid) return;

        var hasError
            = validationResult.Errors.Any(assert => assert.ErrorCode.Equals(errorMessage));
        Assert.That(hasError, Is.True);
    }
    
    protected static void CheckCorrectOutcome<T>(AbstractValidator<T> validator
        , T validatedMethod)
    {
        var validationResult = validator.Validate(validatedMethod);
        Assert.That(validationResult.IsValid, Is.True);
    }
}