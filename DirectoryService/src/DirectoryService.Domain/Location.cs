namespace DirectoryService.Domain;

public class Location
{
    private Location()
    {
        
    }

    private Location(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length < 3 || name.Length > 120)
        {
            throw new Exception("Wrong position name");
        }
        
        Name = name;
    }

    public Guid Id { get; private set; }
    
    public string Name { get; private set; }
    
    public string? Address { get; private set; }
    
    public string? TimeZone { get; private set; }
    
    public bool IsActive { get; private set; }
    
    public DateTime CreatedAt { get; private set; }
    
    public DateTime UpdatedAt { get; private set; }
    
    public List<Guid>? DepartmentId { get; private set; } 
}