using System.Text.RegularExpressions;

namespace DirectoryService.Domain;

/// <summary>
/// Identifier value object
/// </summary>

public class Identifier
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

    public String Value { get; } = null!;

    public static Boolean IsValid(string value)
    {
        return !string.IsNullOrWhiteSpace(value) 
               && value.Length >= 3
               && value.Length <= 150
               && IdentifierRegex.IsMatch(value);
    }
    
    public override bool Equals(object? obj)
    {
        return obj is Name other && StringComparer.OrdinalIgnoreCase.Equals(Value, other.Value);
    }
    
}