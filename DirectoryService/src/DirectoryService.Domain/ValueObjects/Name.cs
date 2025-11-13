using System.Text.RegularExpressions;

namespace DirectoryService.Domain;

/// <summary>
/// Name value object
/// </summary>
public record Name
{
    private static readonly Regex NameRegex = new Regex(@"^[\p{L}\p{M}\p{N}._-]{3,150}$");
    
    public Name(string value)
    {
        if (!IsValid(value))
        {
            throw new ArgumentException("Name is not valid");
        }

        Value = value;
    }
    
    public string Value { get; }

    public static bool IsValid(string value)
    {
        return !string.IsNullOrWhiteSpace(value) 
               && value.Length >= 3
               && value.Length <= 150
               && NameRegex.IsMatch(value);
    }
    
    public bool Equal(object? obj)
    {
        return obj is Name other && StringComparer.OrdinalIgnoreCase.Equals(Value, other.Value);
    }
    
}