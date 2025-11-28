using CSharpFunctionalExtensions;
using Shared;

namespace DirectoryService.Domain.ValueObjects;

public record TimeZone
{
    public TimeZone(string value)
    {
        Value = value;
    }
    
    public string Value { get; private set; }

    public static Result<TimeZone, Error> Create(string value)
    {
        var validate =  Validate(value);
        
        if (validate.IsFailure)
            return GeneralErrors.ValueIsInvalid("Value is invalid");

        return new TimeZone(value);
    }
    
    public static Result<bool,Error> Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return GeneralErrors.ValueIsRequired("TimeZone");

        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(value);
        
        if (timeZone == null)
        {
            return GeneralErrors.ValueIsInvalid("TimeZone");
        }
        
        return true;
    }
}