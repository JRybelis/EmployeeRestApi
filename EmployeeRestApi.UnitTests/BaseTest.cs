using NUnit.Framework;
using FluentValidation;

namespace EmployeeRestApiUnitTests;

public class BaseTest
{
    protected static void CheckError<T>(AbstractValidator<T> validator, int errorCode
        , T validatedMethod)
    {
        var validationResult = validator.Validate(validatedMethod);
        Assert.That(validationResult.IsValid, Is.False);

        if (validationResult.IsValid) return;

        var hasError
            = validationResult.Errors.Any(assert => assert.ErrorCode.Equals(errorCode.ToString()));
        Assert.That(hasError, Is.True);
    }
}