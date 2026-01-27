using FluentValidation.Results;
using Shared;

namespace DirectoryService.Application.Validation;

public static class ValidationExtensions
{
    public static Error ToError(this ValidationResult validationResult)
    {
        var validationErrors = validationResult.Errors;

        var errorMessages = validationErrors.Select(v =>
            new ErrorMessage(
                v.ErrorCode ?? "validation.error",
                v.ErrorMessage,
                v.PropertyName));

        return Error.Validation(errorMessages);
    }
}