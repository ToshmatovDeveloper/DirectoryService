using System.Text.RegularExpressions;

namespace DirectoryService.Domain;

/// <summary>
/// Name value object
/// </summary>
public class Name
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
    
    public String Value { get; }

    public static Boolean IsValid(string value)
    {
        return !string.IsNullOrWhiteSpace(value) 
               && value.Length >= 3
               && value.Length <= 150
               && NameRegex.IsMatch(value);
    }
    
    public override bool Equals(object? obj)
    {
        return obj is Name other && StringComparer.OrdinalIgnoreCase.Equals(Value, other.Value);
    }
    
}