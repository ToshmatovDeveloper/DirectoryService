using System.Text.RegularExpressions;

namespace DirectoryService.Domain.ValueObjects;

/// <summary>
/// Identifier value object
/// </summary>

public record Identifier
{
    private static readonly Regex IdentifierRegex = new Regex(@"^[a-zA-Z0-9._-]{3,150}$");
    
    public Identifier(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException("Identifier is not valid");
        }

        Value = value;
    }

    public string Value { get; } 

    public static bool IsValid(string value)
    {
        return !string.IsNullOrWhiteSpace(value) 
               && value.Length >= 3
               && value.Length <= 150
               && IdentifierRegex.IsMatch(value);
    }
    
    public bool Equal(object? obj)
    {
        return obj is Name other && StringComparer.OrdinalIgnoreCase.Equals(Value, other.Value);
    }
    
}