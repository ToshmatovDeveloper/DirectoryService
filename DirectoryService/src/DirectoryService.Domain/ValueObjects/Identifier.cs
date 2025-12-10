using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using Shared;

namespace DirectoryService.Domain.ValueObjects;

/// <summary>
/// Identifier value object
/// </summary>

public record Identifier
{
    private static readonly Regex IdentifierRegex = new Regex(@"^[a-zA-Z0-9._-]{3,150}$");
    
    public Identifier(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Identifier, Error> Create(string value)
    {
        var validate =  Validate(value);
        
        if (validate.IsFailure)
            return GeneralErrors.ValueIsInvalid("Value is invalid");

        return new Identifier(value);
    }
    
    public static Result<bool,Error> Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length >= 150 
                                             || value.Length <= 3 
                                             || !IdentifierRegex.IsMatch(value))
            return GeneralErrors.ValueIsRequired("Identifier");
        
        return true;
    }
    
    public bool Equal(object? obj)
    {
        return obj is Name other && StringComparer.OrdinalIgnoreCase.Equals(Value, other.Value);
    }
    
}