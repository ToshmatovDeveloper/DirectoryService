namespace DirectoryService.Domain;

/// <summary>
/// Path value object
/// </summary>

public class Path
{
    public Path(string value)
    {
        if (!IsValid(value))
            throw new ArgumentException("Path is not valid");
        
        Value = value;
        
    }
    
    public string Value { get; }

    public static Boolean IsValid(string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }
}