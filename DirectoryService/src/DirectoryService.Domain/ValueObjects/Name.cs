using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Shared;

namespace DirectoryService.Domain.ValueObjects;

/// <summary>
/// Name value object
/// </summary>
public record Name
{
    private static readonly Regex NameRegex = new Regex(@"^[\p{L}\p{M}\p{N}._-]{3,150}$");
    
    public Name(string value)
    {
        Value = value;
    }
    
    public string Value { get; }

    public static Result<Name, Error> Create(string value)
    {
        var validate =  Validate(value);
        
        if (validate.IsFailure)
            return GeneralErrors.ValueIsInvalid("Value is invalid");

        return new Name(value);
    }
    
    public static Result<bool,Error> Validate(string value)
    {
         if (string.IsNullOrWhiteSpace(value) || value.Length >= 150 
                                             || value.Length <= 3 
                                             || !NameRegex.IsMatch(value))
            return GeneralErrors.ValueIsRequired("Name");
        
        return true;
    }
    
    public bool Equal(object? obj)
    {
        return obj is Name other && StringComparer.OrdinalIgnoreCase.Equals(Value, other.Value);
    }
    
}