using CSharpFunctionalExtensions;
using Shared;

namespace DirectoryService.Domain;

/// <summary>
/// Path value object
/// </summary>

public record Path
{
    public Path(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<Path, Error> Create(string value)
    {
        var validate =  Validate(value);
        
        if (validate.IsFailure)
            return GeneralErrors.ValueIsInvalid("Value is invalid");

        return new Path(value);
    }
    
    public static Result<bool,Error> Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return GeneralErrors.ValueIsRequired(null);
      
        return true;
    }
}