namespace DirectoryService.Domain;

public class TimeZone
{
    public TimeZone(string value)
    {
        if (!IsValid(value))
        {
            throw new Exception("Invalid timezone");
        }
        
        Value = value;
    }
    
    public string Value { get; private set; }

    public static bool IsValid(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        try
        {
            _ = TimeZoneInfo.FindSystemTimeZoneById(value);
            return true;
        }
        catch (TimeZoneNotFoundException)
        {
            return false;
        }
        catch  (InvalidTimeZoneException)
        {
            return false;
        }
    }
}